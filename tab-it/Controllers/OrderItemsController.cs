using Microsoft.AspNetCore.Mvc;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

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
}
