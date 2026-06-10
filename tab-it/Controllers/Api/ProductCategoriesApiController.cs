using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.DAL;
using tab_it.Models.Api;
using tab_it.Models.Domain;

namespace tab_it.Controllers.Api;

[Route("api/product-categories")]
[Authorize(Roles = "Admin,Manager")]
public class ProductCategoriesApiController : CrudApiController<ProductCategory, ProductCategoryDto, ProductCategoryWriteDto>
{
    public ProductCategoriesApiController(TabItDbContext db) : base(db) { }

    protected override IQueryable<ProductCategory> ApplySearch(IQueryable<ProductCategory> query, string? q) =>
        string.IsNullOrWhiteSpace(q)
            ? query
            : query.Where(c => c.Name.Contains(q) || c.Description.Contains(q));

    protected override ProductCategoryDto ToDto(ProductCategory entity) => new(entity.Id, entity.Name, entity.Description);

    protected override void Apply(ProductCategory entity, ProductCategoryWriteDto dto)
    {
        entity.Name = dto.Name;
        entity.Description = dto.Description;
    }
}
