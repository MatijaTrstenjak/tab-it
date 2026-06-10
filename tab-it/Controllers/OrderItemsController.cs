using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class OrderItemsController : Controller
{
    private readonly IOrderItemRepository _orderItemRepository;

    public OrderItemsController(IOrderItemRepository orderItemRepository)
    {
        _orderItemRepository = orderItemRepository;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Order Items";
        return View(_orderItemRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var orderItem = _orderItemRepository.GetById(id);
        if (orderItem is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Order Item Details";
        return View(orderItem);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Create Order Item";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(tab_it.Models.Domain.OrderItem orderItem)
    {
        if (ModelState.IsValid)
        {
            _orderItemRepository.Add(orderItem);
            return RedirectToAction(nameof(Index));
        }
        
        ViewData["Title"] = "Create Order Item";
        return View(orderItem);
    }

    public IActionResult Edit(int id)
    {
        var orderItem = _orderItemRepository.GetById(id);
        if (orderItem is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit Order Item";
        return View(orderItem);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var orderItem = _orderItemRepository.GetById(id);
        if (orderItem is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(orderItem);
        if (updated && ModelState.IsValid)
        {
            _orderItemRepository.Update(orderItem);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Edit Order Item";
        return View(orderItem);
    }

    public IActionResult Delete(int id)
    {
        var orderItem = _orderItemRepository.GetById(id);
        if (orderItem is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete Order Item";
        return View(orderItem);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var orderItem = _orderItemRepository.GetById(id);
        if (orderItem is null)
        {
            return NotFound();
        }

        _orderItemRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var orderItems = _orderItemRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            orderItems = orderItems
                .Where(i => i.OrderId.ToString().Contains(query, StringComparison.OrdinalIgnoreCase)
                            || i.ProductId.ToString().Contains(query, StringComparison.OrdinalIgnoreCase)
                            || i.Quantity.ToString().Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_Rows", orderItems);
    }
}
