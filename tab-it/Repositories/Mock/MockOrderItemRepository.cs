using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.Mock;

public class MockOrderItemRepository : IOrderItemRepository
{
    private static readonly IReadOnlyList<OrderItem> OrderItems = new List<OrderItem>
    {
        new()
        {
            Id = 1,
            OrderId = 1,
            ProductId = 1,
            Quantity = 1,
            UnitPrice = 1.80m,
            LineTotal = 1.80m,
            ItemNote = "No sugar"
        },
        new()
        {
            Id = 2,
            OrderId = 1,
            ProductId = 3,
            Quantity = 1,
            UnitPrice = 5.50m,
            LineTotal = 5.50m,
            ItemNote = "Extra sauce"
        },
        new()
        {
            Id = 3,
            OrderId = 2,
            ProductId = 2,
            Quantity = 2,
            UnitPrice = 3.20m,
            LineTotal = 6.40m,
            ItemNote = "With ice"
        },
        new()
        {
            Id = 4,
            OrderId = 3,
            ProductId = 3,
            Quantity = 1,
            UnitPrice = 5.50m,
            LineTotal = 5.50m,
            ItemNote = "No onions"
        }
    };

    public IReadOnlyList<OrderItem> GetAll() => OrderItems;

    public OrderItem? GetById(int id) => OrderItems.SingleOrDefault(oi => oi.Id == id);
}
