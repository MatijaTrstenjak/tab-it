using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.Mock;

public class MockProductRepository : IProductRepository
{
    private static readonly IReadOnlyList<Product> Products = new List<Product>
    {
        new()
        {
            Id = 1,
            Name = "Espresso",
            Sku = "DRINK-ESP-001",
            UnitPrice = 1.80m,
            IsAlcoholic = false,
            AvailableQuantity = 200,
            LastRestockedAt = new DateTime(2026, 3, 28),
            ProductCategoryId = 1
        },
        new()
        {
            Id = 2,
            Name = "Fresh Orange Juice",
            Sku = "DRINK-OJ-002",
            UnitPrice = 3.20m,
            IsAlcoholic = false,
            AvailableQuantity = 80,
            LastRestockedAt = new DateTime(2026, 3, 29),
            ProductCategoryId = 1
        },
        new()
        {
            Id = 3,
            Name = "Club Sandwich",
            Sku = "FOOD-SND-003",
            UnitPrice = 5.50m,
            IsAlcoholic = false,
            AvailableQuantity = 35,
            LastRestockedAt = new DateTime(2026, 3, 27),
            ProductCategoryId = 2
        },
        new()
        {
            Id = 4,
            Name = "Latte",
            Sku = "DRINK-LAT-004",
            UnitPrice = 3.50m,
            IsAlcoholic = false,
            AvailableQuantity = 150,
            LastRestockedAt = new DateTime(2026, 4, 10),
            ProductCategoryId = 1
        },
        new()
        {
            Id = 5,
            Name = "Croissant",
            Sku = "PASTRY-CRO-005",
            UnitPrice = 2.50m,
            IsAlcoholic = false,
            AvailableQuantity = 50,
            LastRestockedAt = new DateTime(2026, 4, 12),
            ProductCategoryId = 3
        },
        new()
        {
            Id = 6,
            Name = "Craft Beer",
            Sku = "ALC-BEER-006",
            UnitPrice = 6.00m,
            IsAlcoholic = true,
            AvailableQuantity = 100,
            LastRestockedAt = new DateTime(2026, 4, 1),
            ProductCategoryId = 4
        },
        new()
        {
            Id = 7,
            Name = "Red Wine Glass",
            Sku = "ALC-WINE-007",
            UnitPrice = 8.50m,
            IsAlcoholic = true,
            AvailableQuantity = 40,
            LastRestockedAt = new DateTime(2026, 4, 5),
            ProductCategoryId = 4
        }
    };

    public IReadOnlyList<Product> GetAll() => Products;

    public Product? GetById(int id) => Products.SingleOrDefault(p => p.Id == id);
}
