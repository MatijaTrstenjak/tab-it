using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class OrderItem
{
    [Key]
    public int Id { get; set; }
    [Range(1, 9999)]
    public int Quantity { get; set; }

    [Range(0, 999999)]
    public decimal UnitPrice { get; set; }

    [Range(0, 999999)]
    public decimal LineTotal { get; set; }

    [StringLength(200)]
    public string ItemNote { get; set; } = string.Empty;

    [ForeignKey("Order")]
    [Range(1, int.MaxValue)]
    public int OrderId { get; set; }
    public virtual Order? Order { get; set; }

    [ForeignKey("Product")]
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }
}
