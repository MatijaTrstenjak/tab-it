using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.DAL;
using tab_it.Models.Api;
using tab_it.Models.Domain;

namespace tab_it.Controllers.Api;

[Route("api/customer-tabs")]
[Authorize(Roles = "Admin,Manager")]
public class CustomerTabsApiController : CrudApiController<CustomerTab, CustomerTabDto, CustomerTabWriteDto>
{
    public CustomerTabsApiController(TabItDbContext db) : base(db) { }

    protected override IQueryable<CustomerTab> ApplySearch(IQueryable<CustomerTab> query, string? q) =>
        string.IsNullOrWhiteSpace(q)
            ? query
            : query.Where(t => t.TabCode.Contains(q) || t.PaymentMethod.Contains(q) || t.Notes.Contains(q));

    protected override CustomerTabDto ToDto(CustomerTab entity) => new(
        entity.Id,
        entity.TabCode,
        entity.TableNumber,
        entity.OpenedAt,
        entity.ClosedAt,
        entity.Status,
        entity.Notes,
        entity.PaymentMethod,
        entity.OpenedByUserId);

    protected override void Apply(CustomerTab entity, CustomerTabWriteDto dto)
    {
        entity.TabCode = dto.TabCode;
        entity.TableNumber = dto.TableNumber;
        entity.OpenedAt = dto.OpenedAt;
        entity.ClosedAt = dto.ClosedAt;
        entity.Status = dto.Status;
        entity.Notes = dto.Notes;
        entity.PaymentMethod = dto.PaymentMethod;
        entity.OpenedByUserId = dto.OpenedByUserId;
    }
}
