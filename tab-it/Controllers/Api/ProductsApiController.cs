using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.DAL;
using tab_it.Models.Api;
using tab_it.Models.Domain;

namespace tab_it.Controllers.Api;

[Route("api/products")]
[Authorize(Roles = "Admin,Manager")]
public class ProductsApiController : CrudApiController<Product, ProductDto, ProductWriteDto>
{
    public ProductsApiController(TabItDbContext db) : base(db) { }

    protected override IQueryable<Product> ApplySearch(IQueryable<Product> query, string? q) =>
        string.IsNullOrWhiteSpace(q)
            ? query
            : query.Where(p => p.Name.Contains(q) || p.Sku.Contains(q));

    protected override ProductDto ToDto(Product entity) => new(
        entity.Id,
        entity.Name,
        entity.Sku,
        entity.UnitPrice,
        entity.IsAlcoholic,
        entity.AvailableQuantity,
        entity.LastRestockedAt,
        entity.ProductCategoryId);

    protected override void Apply(Product entity, ProductWriteDto dto)
    {
        entity.Name = dto.Name;
        entity.Sku = dto.Sku;
        entity.UnitPrice = dto.UnitPrice;
        entity.IsAlcoholic = dto.IsAlcoholic;
        entity.AvailableQuantity = dto.AvailableQuantity;
        entity.LastRestockedAt = dto.LastRestockedAt;
        entity.ProductCategoryId = dto.ProductCategoryId;
    }
}
