namespace tab_it.Models.Domain;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public bool IsAlcoholic { get; set; }
    public int AvailableQuantity { get; set; }
    public DateTime LastRestockedAt { get; set; }

    public int ProductCategoryId { get; set; }
    public ProductCategory? Category { get; set; }

    // Bridge relation for N-N between orders and products.
    public List<OrderItem> OrderItems { get; set; } = new();
}
