using Microsoft.AspNetCore.Mvc;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderRepository _orderRepository;

    public OrdersController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Orders";
        return View(_orderRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var order = _orderRepository.GetById(id);
        if (order is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Order Details";
        return View(order);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Create Order";
        ViewData["CustomerTabDisplay"] = string.Empty;
        return View(new tab_it.Models.Domain.Order
        {
            OrderedAt = DateTime.Now,
            Status = tab_it.Models.Domain.OrderStatus.Draft
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(tab_it.Models.Domain.Order order)
    {
        if (ModelState.IsValid)
        {
            _orderRepository.Add(order);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Create Order";
        ViewData["CustomerTabDisplay"] = Request.Form["CustomerTabDisplay"].ToString();
        return View(order);
    }

    public IActionResult Edit(int id)
    {
        var order = _orderRepository.GetById(id);
        if (order is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit Order";
        ViewData["CustomerTabDisplay"] = GetTabDisplay(order);
        return View(order);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var order = _orderRepository.GetById(id);
        if (order is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(order);
        if (updated && ModelState.IsValid)
        {
            _orderRepository.Update(order);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Edit Order";
        ViewData["CustomerTabDisplay"] = Request.Form["CustomerTabDisplay"].ToString();
        return View(order);
    }

    public IActionResult Delete(int id)
    {
        var order = _orderRepository.GetById(id);
        if (order is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete Order";
        return View(order);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var order = _orderRepository.GetById(id);
        if (order is null)
        {
            return NotFound();
        }

        _orderRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var orders = _orderRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            orders = orders
                .Where(o => o.OrderNumber.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || o.Status.ToString().Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_Rows", orders);
    }

    private static string GetTabDisplay(tab_it.Models.Domain.Order order)
    {
        return order.CustomerTab is null
            ? string.Empty
            : $"{order.CustomerTab.TabCode} (Table {order.CustomerTab.TableNumber})";
    }
}
