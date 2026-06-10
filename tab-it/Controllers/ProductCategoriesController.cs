using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class ProductCategoriesController : Controller
{
    private readonly IProductCategoryRepository _categoryRepository;

    public ProductCategoriesController(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Product Categories";
        return View(_categoryRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var category = _categoryRepository.GetById(id);
        if (category is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Category Details";
        return View(category);
    }

    // GET: ProductCategories/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Create Category";
        return View();
    }

    // POST: ProductCategories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(tab_it.Models.Domain.ProductCategory category)
    {
        if (ModelState.IsValid)
        {
            _categoryRepository.Add(category);
            return RedirectToAction(nameof(Index));
        }
        
        ViewData["Title"] = "Create Category";
        return View(category);
    }

    public IActionResult Edit(int id)
    {
        var category = _categoryRepository.GetById(id);
        if (category is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit Category";
        return View(category);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var category = _categoryRepository.GetById(id);
        if (category is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(category);
        if (updated && ModelState.IsValid)
        {
            _categoryRepository.Update(category);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Edit Category";
        return View(category);
    }

    public IActionResult Delete(int id)
    {
        var category = _categoryRepository.GetById(id);
        if (category is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete Category";
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var category = _categoryRepository.GetById(id);
        if (category is null)
        {
            return NotFound();
        }

        _categoryRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var categories = _categoryRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            categories = categories
                .Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || c.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_Rows", categories);
    }
}
