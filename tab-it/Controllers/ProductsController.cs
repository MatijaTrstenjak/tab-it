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

    public IActionResult Edit(int id)
    {
        var product = _productRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit Product";
        return View(product);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var product = _productRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(product);
        if (updated && ModelState.IsValid)
        {
            _productRepository.Update(product);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Edit Product";
        return View(product);
    }

    public IActionResult Delete(int id)
    {
        var product = _productRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete Product";
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var product = _productRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        _productRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var products = _productRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            products = products
                .Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || p.Sku.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_Rows", products);
    }
}
