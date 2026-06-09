using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using tab_it.Models;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerTabRepository _customerTabRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public HomeController(
            IProductCategoryRepository productCategoryRepository,
            IProductRepository productRepository,
            ICustomerTabRepository customerTabRepository,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IInventoryItemRepository inventoryItemRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _customerTabRepository = customerTabRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        [Route("/")]
        [Route("/pocetna")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var today = DateTime.Today;
            var allOrders = _orderRepository.GetAll()
                .Where(o => o.OrderedAt.Date == today)
                .Where(o => o.CustomerTab is not null && o.CustomerTab.Status == TabStatus.Open)
                .ToList();

            var vm = new DashboardViewModel
            {
                NewOrdersCount = allOrders.Count(o => o.Status == OrderStatus.SentToBar),
                TotalOrdersCount = allOrders.Count,
                WaitingListCount = allOrders.Count(o => o.Status == OrderStatus.Preparing || o.Status == OrderStatus.Ready),
                IncomingOrders = allOrders.Where(o => o.Status == OrderStatus.SentToBar).ToList(),
                OrdersInMaking = allOrders.Where(o => o.Status == OrderStatus.Preparing).ToList(),
                OrdersReady = allOrders.Where(o => o.Status == OrderStatus.Ready).ToList(),
                OrdersServed = allOrders.Where(o => o.Status == OrderStatus.Served).ToList(),
                OpenTabs = _customerTabRepository.GetAll()
                    .Where(t => t.Status == TabStatus.Open)
                    .OrderBy(t => t.TableNumber)
                    .ToList()
            };

            ViewData["Title"] = "Operations Dashboard";
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderStatus(int id, OrderStatus status)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
                return BadRequest();
            }

            var order = _orderRepository.GetById(id);
            if (order is null)
            {
                return NotFound();
            }

            order.Status = status;
            _orderRepository.Update(order);

            return RedirectToAction(nameof(Dashboard));
        }

        public IActionResult POS()
        {
            var categories = _productCategoryRepository.GetAll();
            var products = _productRepository.GetAll();
            var tabs = _customerTabRepository.GetAllForPos();

            ViewData["Categories"] = categories.Select(c => new
            {
                c.Id,
                c.Name,
                c.Description
            }).ToList();
            ViewData["Products"] = products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Sku,
                p.UnitPrice,
                p.AvailableQuantity,
                p.ProductCategoryId,
                p.IsAlcoholic,
                UsesRecipe = p.RecipeItems.Any(),
                CanSell = p.RecipeItems.Any()
                    ? p.RecipeItems.All(r => r.InventoryItem != null && r.InventoryItem.QuantityOnHand >= r.QuantityRequired)
                    : p.AvailableQuantity > 0
            }).ToList();
            ViewData["Tabs"] = tabs.Select(t => new
            {
                t.Id,
                t.TableNumber,
                Status = (int)t.Status,
                Total = t.Orders.Sum(o => o.Total),
                PendingOrders = t.Orders.Count(o => o.Status != OrderStatus.Served && o.Status != OrderStatus.Cancelled),
                CanClose = t.Status == TabStatus.Open
                    && t.Orders.Any()
                    && t.Orders.All(o => o.Status == OrderStatus.Served || o.Status == OrderStatus.Cancelled)
            }).ToList();

            ViewData["Title"] = "Point of Sale";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout([FromBody] PosCheckoutRequest request)
        {
            if (request is null || request.Items is null || request.Items.Count == 0)
            {
                return BadRequest(new { message = "Order is empty." });
            }

            var tab = _customerTabRepository.GetById(request.CustomerTabId);
            if (tab is null)
            {
                return BadRequest(new { message = "Customer tab not found." });
            }

            var orderNumber = $"POS-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 4)}";
            var order = new tab_it.Models.Domain.Order
            {
                OrderNumber = orderNumber,
                OrderedAt = DateTime.Now,
                Status = tab_it.Models.Domain.OrderStatus.SentToBar,
                CustomerTabId = tab.Id,
                DiscountPercent = 0
            };

            decimal subtotal = 0m;
            var orderItems = new List<tab_it.Models.Domain.OrderItem>();
            var productUpdates = new List<Product>();
            var inventoryUsage = new Dictionary<int, decimal>();

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                {
                    return BadRequest(new { message = "Invalid quantity." });
                }

                var product = _productRepository.GetById(item.ProductId);
                if (product is null)
                {
                    return BadRequest(new { message = "Product not found." });
                }

                if (product.RecipeItems.Any())
                {
                    foreach (var recipeItem in product.RecipeItems)
                    {
                        var requiredQuantity = recipeItem.QuantityRequired * item.Quantity;
                        inventoryUsage[recipeItem.InventoryItemId] = inventoryUsage.TryGetValue(recipeItem.InventoryItemId, out var current)
                            ? current + requiredQuantity
                            : requiredQuantity;
                    }
                }
                else if (product.AvailableQuantity < item.Quantity)
                {
                    return BadRequest(new { message = $"Insufficient stock for {product.Name}." });
                }

                var lineTotal = product.UnitPrice * item.Quantity;
                subtotal += lineTotal;

                if (!product.RecipeItems.Any())
                {
                    product.AvailableQuantity -= item.Quantity;
                    productUpdates.Add(product);
                }

                orderItems.Add(new tab_it.Models.Domain.OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice,
                    LineTotal = lineTotal,
                    ItemNote = string.Empty
                });
            }

            var inventoryUpdates = new List<InventoryItem>();
            foreach (var usage in inventoryUsage)
            {
                var inventoryItem = _inventoryItemRepository.GetById(usage.Key);
                if (inventoryItem is null)
                {
                    return BadRequest(new { message = "Recipe inventory item not found." });
                }

                if (inventoryItem.QuantityOnHand < usage.Value)
                {
                    return BadRequest(new { message = $"Insufficient inventory for {inventoryItem.Name}. Need {usage.Value:0.###} {inventoryItem.Unit}, have {inventoryItem.QuantityOnHand:0.###}." });
                }

                inventoryItem.QuantityOnHand -= usage.Value;
                inventoryUpdates.Add(inventoryItem);
            }

            foreach (var product in productUpdates)
            {
                _productRepository.Update(product);
            }

            foreach (var inventoryItem in inventoryUpdates)
            {
                _inventoryItemRepository.Update(inventoryItem);
            }

            order.Subtotal = subtotal;
            order.Total = subtotal;

            _orderRepository.Add(order);

            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = order.Id;
                _orderItemRepository.Add(orderItem);
            }

            return Ok(new
            {
                orderId = order.Id,
                orderNumber = order.OrderNumber,
                total = order.Total
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTab([FromBody] PosCreateTabRequest request)
        {
            if (request is null || request.TableNumber <= 0)
            {
                return BadRequest(new { message = "Table number is required." });
            }

            var existingOpenTab = _customerTabRepository.GetAllBasic()
                .FirstOrDefault(t => t.TableNumber == request.TableNumber && t.Status == TabStatus.Open);
            if (existingOpenTab is not null)
            {
                return BadRequest(new { message = $"Table {request.TableNumber} already has an open tab." });
            }

            var tabCode = $"TAB-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 4)}";
            var tab = new tab_it.Models.Domain.CustomerTab
            {
                TabCode = tabCode,
                TableNumber = request.TableNumber,
                Notes = request.Notes?.Trim() ?? string.Empty,
                OpenedAt = DateTime.Now,
                Status = tab_it.Models.Domain.TabStatus.Open,
                OpenedByUserId = 1
            };

            _customerTabRepository.Add(tab);

            return Ok(new
            {
                id = tab.Id,
                tableNumber = tab.TableNumber,
                status = (int)tab.Status,
                total = 0m,
                pendingOrders = 0,
                canClose = false
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTabStatus([FromBody] PosUpdateTabStatusRequest request)
        {
            if (request is null)
            {
                return BadRequest(new { message = "Invalid request." });
            }

            var tab = _customerTabRepository.GetById(request.TabId);
            if (tab is null)
            {
                return BadRequest(new { message = "Customer tab not found." });
            }

            if (!Enum.IsDefined(typeof(tab_it.Models.Domain.TabStatus), request.Status))
            {
                return BadRequest(new { message = "Invalid status." });
            }

            var status = (tab_it.Models.Domain.TabStatus)request.Status;
            tab.Status = status;
            tab.ClosedAt = status == tab_it.Models.Domain.TabStatus.Closed
                ? DateTime.Now
                : null;

            _customerTabRepository.Update(tab);

            return Ok(new
            {
                id = tab.Id,
                status = (int)tab.Status
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CloseTab([FromBody] PosCloseTabRequest request)
        {
            if (request is null || request.TabId <= 0)
            {
                return BadRequest(new { message = "Invalid tab." });
            }

            var paymentMethod = (request.PaymentMethod ?? string.Empty).Trim();
            var allowedPayments = new[] { "Cash", "Credit Card", "Debit Card" };
            if (!allowedPayments.Contains(paymentMethod, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "Select a valid payment method." });
            }

            paymentMethod = allowedPayments.First(p => p.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));

            var tab = _customerTabRepository.GetById(request.TabId);
            if (tab is null)
            {
                return BadRequest(new { message = "Customer tab not found." });
            }

            if (tab.Status != TabStatus.Open)
            {
                return BadRequest(new { message = "This table is already closed." });
            }

            if (!tab.Orders.Any())
            {
                return BadRequest(new { message = "Cannot close an empty tab." });
            }

            var pendingOrders = tab.Orders.Count(o => o.Status != OrderStatus.Served && o.Status != OrderStatus.Cancelled);
            if (pendingOrders > 0)
            {
                return BadRequest(new { message = "Serve every order before closing this tab." });
            }

            tab.Status = TabStatus.Closed;
            tab.ClosedAt = DateTime.Now;
            _customerTabRepository.Update(tab);

            var pdf = CreateReceiptPdf(tab, paymentMethod);
            var fileName = $"receipt-table-{tab.TableNumber}-{DateTime.Now:yyyyMMdd-HHmm}.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        public class PosCheckoutRequest
        {
            public int CustomerTabId { get; set; }
            public List<PosCheckoutItem> Items { get; set; } = new();
        }

        public class PosCheckoutItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public class PosCreateTabRequest
        {
            public int TableNumber { get; set; }
            public string? Notes { get; set; }
        }

        public class PosUpdateTabStatusRequest
        {
            public int TabId { get; set; }
            public int Status { get; set; }
        }

        public class PosCloseTabRequest
        {
            public int TabId { get; set; }
            public string? PaymentMethod { get; set; }
        }

        private static byte[] CreateReceiptPdf(CustomerTab tab, string paymentMethod)
        {
            var lines = new List<string>
            {
                "tab_it receipt",
                $"Table {tab.TableNumber}",
                $"Closed: {DateTime.Now:yyyy-MM-dd HH:mm}",
                $"Payment: {paymentMethod}",
                " "
            };

            foreach (var order in tab.Orders.OrderBy(o => o.OrderedAt))
            {
                lines.Add($"Order {order.OrderedAt:HH:mm}");
                foreach (var item in order.Items)
                {
                    var name = item.Product?.Name ?? $"Product {item.ProductId}";
                    lines.Add($"{item.Quantity} x {name}    {item.LineTotal:F2} EUR");
                }
            }

            lines.Add(" ");
            lines.Add($"TOTAL    {tab.Orders.Sum(o => o.Total):F2} EUR");

            var content = new StringBuilder();
            content.AppendLine("BT");
            content.AppendLine("/F1 11 Tf");
            content.AppendLine("50 790 Td");

            foreach (var line in lines)
            {
                content.AppendLine($"({EscapePdfText(line)}) Tj");
                content.AppendLine("0 -16 Td");
            }

            content.AppendLine("ET");

            var objects = new List<string>
            {
                "<< /Type /Catalog /Pages 2 0 R >>",
                "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
                "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>",
                "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>",
                $"<< /Length {Encoding.ASCII.GetByteCount(content.ToString())} >>\nstream\n{content}endstream"
            };

            var pdf = new StringBuilder();
            var offsets = new List<int> { 0 };
            pdf.AppendLine("%PDF-1.4");

            for (var i = 0; i < objects.Count; i++)
            {
                offsets.Add(Encoding.ASCII.GetByteCount(pdf.ToString()));
                pdf.AppendLine($"{i + 1} 0 obj");
                pdf.AppendLine(objects[i]);
                pdf.AppendLine("endobj");
            }

            var xrefOffset = Encoding.ASCII.GetByteCount(pdf.ToString());
            pdf.AppendLine("xref");
            pdf.AppendLine($"0 {objects.Count + 1}");
            pdf.AppendLine("0000000000 65535 f ");
            foreach (var offset in offsets.Skip(1))
            {
                pdf.AppendLine($"{offset:0000000000} 00000 n ");
            }

            pdf.AppendLine("trailer");
            pdf.AppendLine($"<< /Size {objects.Count + 1} /Root 1 0 R >>");
            pdf.AppendLine("startxref");
            pdf.AppendLine(xrefOffset.ToString());
            pdf.AppendLine("%%EOF");

            return Encoding.ASCII.GetBytes(pdf.ToString());
        }

        private static string EscapePdfText(string text)
        {
            return text
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
