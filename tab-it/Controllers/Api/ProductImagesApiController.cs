using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tab_it.DAL;
using tab_it.Models.Api;
using tab_it.Models.Domain;

namespace tab_it.Controllers.Api;

[ApiController]
[Route("api/products/{productId:int}/images")]
[Authorize(Roles = "Admin,Manager")]
public class ProductImagesApiController : ControllerBase
{
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp",
        "image/gif"
    };

    private readonly TabItDbContext _db;
    private readonly IWebHostEnvironment _environment;

    public ProductImagesApiController(TabItDbContext db, IWebHostEnvironment environment)
    {
        _db = db;
        _environment = environment;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductImageDto>>> GetAll(int productId)
    {
        if (!await _db.Products.AnyAsync(p => p.Id == productId))
        {
            return NotFound();
        }

        var images = await _db.ProductImages
            .AsNoTracking()
            .Where(i => i.ProductId == productId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

        return Ok(images.Select(ToDto));
    }

    [HttpGet("{imageId:int}")]
    public async Task<ActionResult<ProductImageDto>> GetById(int productId, int imageId)
    {
        var image = await _db.ProductImages
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.Id == imageId);

        return image is null ? NotFound() : Ok(ToDto(image));
    }

    [HttpPost]
    [RequestSizeLimit(10_000_000)]
    public async Task<ActionResult<ProductImageDto>> Upload(int productId, IFormFile? file)
    {
        if (!await _db.Products.AnyAsync(p => p.Id == productId))
        {
            return NotFound();
        }

        if (!IsValidImage(file))
        {
            return BadRequest("Upload a non-empty JPEG, PNG, WEBP, or GIF image.");
        }

        var uploadedFile = file!;
        var savedFile = await SaveFileAsync(productId, uploadedFile);

        var image = new ProductImage
        {
            ProductId = productId,
            OriginalFileName = Path.GetFileName(uploadedFile.FileName),
            StoredFileName = savedFile.StoredFileName,
            RelativePath = savedFile.RelativePath,
            ContentType = uploadedFile.ContentType,
            FileSize = uploadedFile.Length,
            CreatedAt = DateTime.UtcNow
        };

        _db.ProductImages.Add(image);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { productId, imageId = image.Id }, ToDto(image));
    }

    [HttpPut("{imageId:int}")]
    [RequestSizeLimit(10_000_000)]
    public async Task<ActionResult<ProductImageDto>> Replace(int productId, int imageId, IFormFile? file)
    {
        var image = await _db.ProductImages.FirstOrDefaultAsync(i => i.ProductId == productId && i.Id == imageId);
        if (image is null)
        {
            return NotFound();
        }

        if (!IsValidImage(file))
        {
            return BadRequest("Upload a non-empty JPEG, PNG, WEBP, or GIF image.");
        }

        TryDeletePhysicalFile(image.RelativePath);
        var uploadedFile = file!;
        var savedFile = await SaveFileAsync(productId, uploadedFile);

        image.OriginalFileName = Path.GetFileName(uploadedFile.FileName);
        image.StoredFileName = savedFile.StoredFileName;
        image.RelativePath = savedFile.RelativePath;
        image.ContentType = uploadedFile.ContentType;
        image.FileSize = uploadedFile.Length;
        image.CreatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(ToDto(image));
    }

    [HttpDelete("{imageId:int}")]
    public async Task<IActionResult> Delete(int productId, int imageId)
    {
        var image = await _db.ProductImages.FirstOrDefaultAsync(i => i.ProductId == productId && i.Id == imageId);
        if (image is null)
        {
            return NotFound();
        }

        image.IsDeleted = true;
        image.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static ProductImageDto ToDto(ProductImage image) => new(
        image.Id,
        image.ProductId,
        image.OriginalFileName,
        image.StoredFileName,
        image.RelativePath,
        image.ContentType,
        image.FileSize,
        image.CreatedAt);

    private static bool IsValidImage(IFormFile? file) =>
        file is not null &&
        file.Length > 0 &&
        AllowedContentTypes.Contains(file.ContentType);

    private async Task<SavedProductFile> SaveFileAsync(int productId, IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var relativeDirectory = Path.Combine("uploads", "products", productId.ToString());
        var webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
        var absoluteDirectory = Path.Combine(webRootPath, relativeDirectory);
        Directory.CreateDirectory(absoluteDirectory);

        var absolutePath = Path.Combine(absoluteDirectory, storedFileName);
        await using (var stream = System.IO.File.Create(absolutePath))
        {
            await file.CopyToAsync(stream);
        }

        return new SavedProductFile(
            storedFileName,
            "/" + Path.Combine(relativeDirectory, storedFileName).Replace("\\", "/"));
    }

    private void TryDeletePhysicalFile(string relativePath)
    {
        var webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
        var normalizedRelativePath = relativePath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
        var absolutePath = Path.Combine(webRootPath, normalizedRelativePath);

        if (System.IO.File.Exists(absolutePath))
        {
            System.IO.File.Delete(absolutePath);
        }
    }

    private sealed record SavedProductFile(string StoredFileName, string RelativePath);
}
