using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.Mock;

public class MockProductCategoryRepository : IProductCategoryRepository
{
    private static readonly IReadOnlyList<ProductCategory> Categories = new List<ProductCategory>
    {
        new()
        {
            Id = 1,
            Name = "Drinks",
            Description = "Hot and cold drinks"
        },
        new()
        {
            Id = 2,
            Name = "Food",
            Description = "Main and snack dishes"
        },
        new()
        {
            Id = 3,
            Name = "Pastries",
            Description = "Sweet baked goods"
        },
        new()
        {
            Id = 4,
            Name = "Alcohol",
            Description = "Beer, wine, and cocktails"
        }
    };

    public IReadOnlyList<ProductCategory> GetAll() => Categories;

    public ProductCategory? GetById(int id) => Categories.SingleOrDefault(c => c.Id == id);
}
