using System.ComponentModel.DataAnnotations;

namespace tab_it.Models.Domain;

public class InventoryItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(60)]
    public string Sku { get; set; } = string.Empty;

    public InventoryUnit Unit { get; set; }

    [Range(0, 999999)]
    public decimal QuantityOnHand { get; set; }

    [Range(0, 999999)]
    public decimal ReorderLevel { get; set; }

    public DateTime LastRestockedAt { get; set; }

    public virtual ICollection<ProductRecipeItem> RecipeItems { get; set; } = new List<ProductRecipeItem>();
}
