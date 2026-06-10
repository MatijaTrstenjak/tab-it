using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.DAL;
using tab_it.Models.Api;
using tab_it.Models.Domain;

namespace tab_it.Controllers.Api;

[Route("api/order-items")]
[Authorize(Roles = "Admin,Manager")]
public class OrderItemsApiController : CrudApiController<OrderItem, OrderItemDto, OrderItemWriteDto>
{
    public OrderItemsApiController(TabItDbContext db) : base(db) { }

    protected override OrderItemDto ToDto(OrderItem entity) => new(
        entity.Id,
        entity.Quantity,
        entity.UnitPrice,
        entity.LineTotal,
        entity.ItemNote,
        entity.OrderId,
        entity.ProductId);

    protected override void Apply(OrderItem entity, OrderItemWriteDto dto)
    {
        entity.Quantity = dto.Quantity;
        entity.UnitPrice = dto.UnitPrice;
        entity.LineTotal = dto.LineTotal;
        entity.ItemNote = dto.ItemNote;
        entity.OrderId = dto.OrderId;
        entity.ProductId = dto.ProductId;
    }
}
