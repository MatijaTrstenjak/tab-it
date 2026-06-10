using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.DAL;
using tab_it.Models.Api;
using tab_it.Models.Domain;

namespace tab_it.Controllers.Api;

[Route("api/inventory-items")]
[Authorize(Roles = "Admin,Manager")]
public class InventoryItemsApiController : CrudApiController<InventoryItem, InventoryItemDto, InventoryItemWriteDto>
{
    public InventoryItemsApiController(TabItDbContext db) : base(db) { }

    protected override IQueryable<InventoryItem> ApplySearch(IQueryable<InventoryItem> query, string? q) =>
        string.IsNullOrWhiteSpace(q)
            ? query
            : query.Where(i => i.Name.Contains(q) || i.Sku.Contains(q));

    protected override InventoryItemDto ToDto(InventoryItem entity) => new(
        entity.Id,
        entity.Name,
        entity.Sku,
        entity.Unit,
        entity.QuantityOnHand,
        entity.ReorderLevel,
        entity.LastRestockedAt);

    protected override void Apply(InventoryItem entity, InventoryItemWriteDto dto)
    {
        entity.Name = dto.Name;
        entity.Sku = dto.Sku;
        entity.Unit = dto.Unit;
        entity.QuantityOnHand = dto.QuantityOnHand;
        entity.ReorderLevel = dto.ReorderLevel;
        entity.LastRestockedAt = dto.LastRestockedAt;
    }
}
