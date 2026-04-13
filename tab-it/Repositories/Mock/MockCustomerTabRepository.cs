using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.Mock;

public class MockCustomerTabRepository : ICustomerTabRepository
{
    private static readonly IReadOnlyList<CustomerTab> Tabs = new List<CustomerTab>
    {
        new()
        {
            Id = 1,
            TabCode = "TAB-001",
            TableNumber = 7,
            OpenedAt = new DateTime(2026, 3, 30, 18, 10, 0),
            Status = TabStatus.Open,
            Notes = "Birthday group",
            OpenedByUserId = 2
        },
        new()
        {
            Id = 2,
            TabCode = "TAB-002",
            TableNumber = 3,
            OpenedAt = new DateTime(2026, 3, 30, 18, 25, 0),
            Status = TabStatus.Open,
            Notes = "Allergic to nuts",
            OpenedByUserId = 3
        },
        new()
        {
            Id = 3,
            TabCode = "TAB-003",
            TableNumber = 12,
            OpenedAt = new DateTime(2026, 3, 30, 17, 40, 0),
            ClosedAt = new DateTime(2026, 3, 30, 19, 00, 0),
            Status = TabStatus.Closed,
            Notes = "Paid by card",
            OpenedByUserId = 2
        }
    };

    public IReadOnlyList<CustomerTab> GetAll() => Tabs;

    public CustomerTab? GetById(int id) => Tabs.SingleOrDefault(t => t.Id == id);
}
