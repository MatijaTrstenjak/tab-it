using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.DAL;
using tab_it.Models.Api;
using tab_it.Models.Domain;

namespace tab_it.Controllers.Api;

[Route("api/product-recipe-items")]
[Authorize(Roles = "Admin,Manager")]
public class ProductRecipeItemsApiController : CrudApiController<ProductRecipeItem, ProductRecipeItemDto, ProductRecipeItemWriteDto>
{
    public ProductRecipeItemsApiController(TabItDbContext db) : base(db) { }

    protected override ProductRecipeItemDto ToDto(ProductRecipeItem entity) => new(
        entity.Id,
        entity.ProductId,
        entity.InventoryItemId,
        entity.QuantityRequired);

    protected override void Apply(ProductRecipeItem entity, ProductRecipeItemWriteDto dto)
    {
        entity.ProductId = dto.ProductId;
        entity.InventoryItemId = dto.InventoryItemId;
        entity.QuantityRequired = dto.QuantityRequired;
    }
}
