using Microsoft.AspNetCore.Mvc;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

public class ProductsController : Controller
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [Route("/proizvodi/katalog")]
    [Route("/Products/Index")] // fallback for classic links
    [Route("/Products")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Products";
        return View(_productRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var product = _productRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Product Details";
        return View(product);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Create Product";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(tab_it.Models.Domain.Product product)
    {
        if (ModelState.IsValid)
        {
            _productRepository.Add(product);
            return RedirectToAction(nameof(Index));
        }
        
        ViewData["Title"] = "Create Product";
        return View(product);
    }
}
