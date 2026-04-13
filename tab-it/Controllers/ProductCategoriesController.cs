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
}
