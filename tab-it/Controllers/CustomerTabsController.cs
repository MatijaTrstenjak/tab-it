using Microsoft.AspNetCore.Mvc;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

public class CustomerTabsController : Controller
{
    private readonly ICustomerTabRepository _tabRepository;

    public CustomerTabsController(ICustomerTabRepository tabRepository)
    {
        _tabRepository = tabRepository;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Customer Tabs";
        return View(_tabRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var tab = _tabRepository.GetById(id);
        if (tab is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Tab Details";
        return View(tab);
    }
}
