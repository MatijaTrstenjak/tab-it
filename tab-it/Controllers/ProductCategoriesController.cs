using Microsoft.AspNetCore.Mvc;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

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
}
