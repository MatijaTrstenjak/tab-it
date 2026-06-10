using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.DAL;
using tab_it.Models.Api;
using tab_it.Models.Domain;

namespace tab_it.Controllers.Api;

[Route("api/orders")]
[Authorize(Roles = "Admin,Manager")]
public class OrdersApiController : CrudApiController<Order, OrderDto, OrderWriteDto>
{
    public OrdersApiController(TabItDbContext db) : base(db) { }

    protected override IQueryable<Order> ApplySearch(IQueryable<Order> query, string? q) =>
        string.IsNullOrWhiteSpace(q)
            ? query
            : query.Where(o => o.OrderNumber.Contains(q));

    protected override OrderDto ToDto(Order entity) => new(
        entity.Id,
        entity.OrderNumber,
        entity.OrderedAt,
        entity.Status,
        entity.Subtotal,
        entity.DiscountPercent,
        entity.Total,
        entity.CustomerTabId);

    protected override void Apply(Order entity, OrderWriteDto dto)
    {
        entity.OrderNumber = dto.OrderNumber;
        entity.OrderedAt = dto.OrderedAt;
        entity.Status = dto.Status;
        entity.Subtotal = dto.Subtotal;
        entity.DiscountPercent = dto.DiscountPercent;
        entity.Total = dto.Total;
        entity.CustomerTabId = dto.CustomerTabId;
    }
}
