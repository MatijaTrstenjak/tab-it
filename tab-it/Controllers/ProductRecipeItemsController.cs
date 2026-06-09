using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

public class ProductRecipeItemsController : Controller
{
    private readonly IProductRecipeItemRepository _recipeRepository;
    private readonly IProductRepository _productRepository;
    private readonly IInventoryItemRepository _inventoryItemRepository;

    public ProductRecipeItemsController(
        IProductRecipeItemRepository recipeRepository,
        IProductRepository productRepository,
        IInventoryItemRepository inventoryItemRepository)
    {
        _recipeRepository = recipeRepository;
        _productRepository = productRepository;
        _inventoryItemRepository = inventoryItemRepository;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Recipes";
        return View(_recipeRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var item = _recipeRepository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Recipe Details";
        return View(item);
    }

    public IActionResult RecipeDetails(int id)
    {
        var product = _productRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Recipe Details";
        return View(product);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Create Recipe Item";
        PopulateSelectLists();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductRecipeItem item)
    {
        if (ModelState.IsValid)
        {
            _recipeRepository.Add(item);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Create Recipe Item";
        PopulateSelectLists(item.ProductId, item.InventoryItemId);
        return View(item);
    }

    public IActionResult Edit(int id)
    {
        var item = _recipeRepository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit Recipe Item";
        PopulateSelectLists(item.ProductId, item.InventoryItemId);
        return View(item);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var item = _recipeRepository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(item);
        if (updated && ModelState.IsValid)
        {
            _recipeRepository.Update(item);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Edit Recipe Item";
        PopulateSelectLists(item.ProductId, item.InventoryItemId);
        return View(item);
    }

    public IActionResult Delete(int id)
    {
        var item = _recipeRepository.GetById(id);
        if (item is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete Recipe Item";
        return View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _recipeRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var items = _recipeRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            items = items
                .Where(i => (i.Product?.Name ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase)
                            || (i.InventoryItem?.Name ?? string.Empty).Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_Rows", items);
    }

    private void PopulateSelectLists(int? productId = null, int? inventoryItemId = null)
    {
        ViewData["Products"] = new SelectList(_productRepository.GetAll(), "Id", "Name", productId);
        ViewData["InventoryItems"] = new SelectList(_inventoryItemRepository.GetAll(), "Id", "Name", inventoryItemId);
    }
}
