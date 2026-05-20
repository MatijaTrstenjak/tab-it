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
        return View(new tab_it.Models.Domain.CustomerTab
        {
            OpenedAt = DateTime.Now,
            Status = tab_it.Models.Domain.TabStatus.Open
        });
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

    public IActionResult Edit(int id)
    {
        var tab = _tabRepository.GetById(id);
        if (tab is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit Tab";
        return View(tab);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var tab = _tabRepository.GetById(id);
        if (tab is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(tab);
        if (updated && ModelState.IsValid)
        {
            _tabRepository.Update(tab);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Edit Tab";
        return View(tab);
    }

    public IActionResult Delete(int id)
    {
        var tab = _tabRepository.GetById(id);
        if (tab is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete Tab";
        return View(tab);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var tab = _tabRepository.GetById(id);
        if (tab is null)
        {
            return NotFound();
        }

        _tabRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var tabs = _tabRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            tabs = tabs
                .Where(t => t.TabCode.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || t.TableNumber.ToString().Contains(query, StringComparison.OrdinalIgnoreCase)
                            || t.Status.ToString().Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_Rows", tabs);
    }

    [HttpGet]
    public IActionResult Lookup(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var tabs = _tabRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            tabs = tabs
                .Where(t => t.TabCode.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || t.TableNumber.ToString().Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var results = tabs
            .Take(12)
            .Select(t => new
            {
                id = t.Id,
                text = $"{t.TabCode} (Table {t.TableNumber})"
            });

        return Json(results);
    }
}
