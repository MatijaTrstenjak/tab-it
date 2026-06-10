using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class ProductsController : Controller
{
    private const long MaxImageBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp",
        ".gif"
    };

    private readonly IProductRepository _productRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductsController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
    {
        _productRepository = productRepository;
        _webHostEnvironment = webHostEnvironment;
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
        return View(new Product());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        ValidateProductImage(imageFile);
        if (ModelState.IsValid)
        {
            _productRepository.Add(product);
            var productImage = await SaveProductImageAsync(imageFile, product.Id);
            if (productImage is not null)
            {
                product.Images.Add(productImage);
                _productRepository.Update(product);
            }

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
    public async Task<IActionResult> EditPost(int id, IFormFile? imageFile)
    {
        var product = _productRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        ValidateProductImage(imageFile);

        var updated = await TryUpdateModelAsync(
            product,
            string.Empty,
            p => p.Name,
            p => p.Sku,
            p => p.UnitPrice,
            p => p.IsAlcoholic,
            p => p.AvailableQuantity,
            p => p.ProductCategoryId);
        if (updated && ModelState.IsValid)
        {
            var productImage = await SaveProductImageAsync(imageFile, product.Id);
            if (productImage is not null)
            {
                foreach (var existingImage in product.Images.Where(i => !i.IsDeleted))
                {
                    existingImage.IsDeleted = true;
                    existingImage.DeletedAt = DateTime.UtcNow;
                }

                product.Images.Add(productImage);
            }

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

    private void ValidateProductImage(IFormFile? imageFile)
    {
        if (imageFile is null || imageFile.Length == 0)
        {
            return;
        }

        var extension = Path.GetExtension(imageFile.FileName);
        if (!AllowedImageExtensions.Contains(extension))
        {
            ModelState.AddModelError("ImageFile", "Upload a JPG, PNG, WebP, or GIF image.");
            return;
        }

        if (!imageFile.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError("ImageFile", "The uploaded file must be an image.");
            return;
        }

        if (imageFile.Length > MaxImageBytes)
        {
            ModelState.AddModelError("ImageFile", "Upload an image smaller than 5 MB.");
        }
    }

    private async Task<ProductImage?> SaveProductImageAsync(IFormFile? imageFile, int productId)
    {
        if (imageFile is null || imageFile.Length == 0 || !ModelState.IsValid)
        {
            return null;
        }

        var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "menu-items");
        Directory.CreateDirectory(uploadFolder);

        var extension = Path.GetExtension(imageFile.FileName);
        var storedFileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var physicalPath = Path.Combine(uploadFolder, storedFileName);

        await using (var stream = System.IO.File.Create(physicalPath))
        {
            await imageFile.CopyToAsync(stream);
        }

        return new ProductImage
        {
            ProductId = productId,
            OriginalFileName = Path.GetFileName(imageFile.FileName),
            StoredFileName = storedFileName,
            RelativePath = $"/uploads/menu-items/{storedFileName}",
            ContentType = imageFile.ContentType,
            FileSize = imageFile.Length,
            CreatedAt = DateTime.UtcNow
        };
    }
}
