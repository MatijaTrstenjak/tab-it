using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public bool IsAlcoholic { get; set; }
    public int AvailableQuantity { get; set; }
    public DateTime LastRestockedAt { get; set; }

    [ForeignKey("Category")]
    public int ProductCategoryId { get; set; }
    public virtual ProductCategory? Category { get; set; }

    // Bridge relation for N-N between orders and products.
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
