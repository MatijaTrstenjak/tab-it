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

    [Route("/racuni/detalji/{id}")]
    [Route("/CustomerTabs/Details/{id}")]
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

    public IActionResult Create()
    {
        ViewData["Title"] = "Create Tab";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(tab_it.Models.Domain.CustomerTab tab)
    {
        if (ModelState.IsValid)
        {
            _tabRepository.Add(tab);
            return RedirectToAction(nameof(Index));
        }
        
        ViewData["Title"] = "Create Tab";
        return View(tab);
    }
}
