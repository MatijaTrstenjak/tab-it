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
}
