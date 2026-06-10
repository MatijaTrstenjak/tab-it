using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class Product : ISoftDeletable
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(60)]
    public string Sku { get; set; } = string.Empty;

    [Range(0, 999999)]
    public decimal UnitPrice { get; set; }
    public bool IsAlcoholic { get; set; }
    [Range(0, 999999)]
    public int AvailableQuantity { get; set; }
    public DateTime LastRestockedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("Category")]
    [Range(1, int.MaxValue)]
    public int ProductCategoryId { get; set; }
    public virtual ProductCategory? Category { get; set; }

    // Bridge relation for N-N between orders and products.
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<ProductRecipeItem> RecipeItems { get; set; } = new List<ProductRecipeItem>();
    public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
}
