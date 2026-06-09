using Microsoft.AspNetCore.Mvc;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

public class InventoryItemsController : Controller
{
    private readonly IInventoryItemRepository _inventoryItemRepository;

    public InventoryItemsController(IInventoryItemRepository inventoryItemRepository)
    {
        _inventoryItemRepository = inventoryItemRepository;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Inventory";
        return View(_inventoryItemRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var item = _inventoryItemRepository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Inventory Details";
        return View(item);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Create Inventory Item";
        return View(new InventoryItem
        {
            Unit = InventoryUnit.Quantity,
            LastRestockedAt = DateTime.Now
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(InventoryItem item)
    {
        if (ModelState.IsValid)
        {
            _inventoryItemRepository.Add(item);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Create Inventory Item";
        return View(item);
    }

    public IActionResult Edit(int id)
    {
        var item = _inventoryItemRepository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit Inventory Item";
        return View(item);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var item = _inventoryItemRepository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(item);
        if (updated && ModelState.IsValid)
        {
            _inventoryItemRepository.Update(item);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Edit Inventory Item";
        return View(item);
    }

    public IActionResult Delete(int id)
    {
        var item = _inventoryItemRepository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete Inventory Item";
        return View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _inventoryItemRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var items = _inventoryItemRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            items = items
                .Where(i => i.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || i.Sku.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || i.Unit.ToString().Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_Rows", items);
    }
}
