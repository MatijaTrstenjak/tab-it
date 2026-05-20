using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using tab_it.Models;
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

        public HomeController(
            IProductCategoryRepository productCategoryRepository,
            IProductRepository productRepository,
            ICustomerTabRepository customerTabRepository,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _customerTabRepository = customerTabRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        [Route("/")]
        [Route("/pocetna")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Operations Dashboard";
            return View();
        }

        public IActionResult POS()
        {
            var categories = _productCategoryRepository.GetAll();
            var products = _productRepository.GetAll();
            var tabs = _customerTabRepository.GetAll();

            ViewData["Categories"] = categories;
            ViewData["Products"] = products;
            ViewData["Tabs"] = tabs;

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

                if (product.AvailableQuantity < item.Quantity)
                {
                    return BadRequest(new { message = $"Insufficient stock for {product.Name}." });
                }

                var lineTotal = product.UnitPrice * item.Quantity;
                subtotal += lineTotal;

                product.AvailableQuantity -= item.Quantity;
                _productRepository.Update(product);

                orderItems.Add(new tab_it.Models.Domain.OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice,
                    LineTotal = lineTotal,
                    ItemNote = string.Empty
                });
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
                tabCode = tab.TabCode,
                tableNumber = tab.TableNumber,
                status = (int)tab.Status
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
