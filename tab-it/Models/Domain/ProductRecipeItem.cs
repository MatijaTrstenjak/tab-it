using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class ProductRecipeItem
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Product")]
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }

    [ForeignKey("InventoryItem")]
    [Range(1, int.MaxValue)]
    public int InventoryItemId { get; set; }
    public virtual InventoryItem? InventoryItem { get; set; }

    [Range(0.0001, 999999)]
    public decimal QuantityRequired { get; set; }
}
