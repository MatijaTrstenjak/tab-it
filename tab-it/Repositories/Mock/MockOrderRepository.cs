using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.Mock;

public class MockOrderRepository : IOrderRepository
{
    private static readonly IReadOnlyList<Order> Orders = new List<Order>
    {
        new()
        {
            Id = 1,
            OrderNumber = "ORD-001",
            OrderedAt = new DateTime(2026, 3, 30, 18, 15, 0),
            Status = OrderStatus.Served,
            Subtotal = 7.30m,
            DiscountPercent = 0m,
            Total = 7.30m,
            CustomerTabId = 1
        },
        new()
        {
            Id = 2,
            OrderNumber = "ORD-002",
            OrderedAt = new DateTime(2026, 3, 30, 18, 35, 0),
            Status = OrderStatus.Ready,
            Subtotal = 11.00m,
            DiscountPercent = 10m,
            Total = 9.90m,
            CustomerTabId = 2
        },
        new()
        {
            Id = 3,
            OrderNumber = "ORD-003",
            OrderedAt = new DateTime(2026, 3, 30, 17, 55, 0),
            Status = OrderStatus.Served,
            Subtotal = 5.50m,
            DiscountPercent = 0m,
            Total = 5.50m,
            CustomerTabId = 3
        }
    };

    public IReadOnlyList<Order> GetAll() => Orders;

    public Order? GetById(int id) => Orders.SingleOrDefault(o => o.Id == id);
}
