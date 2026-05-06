using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class OrderItem
{
    [Key]
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
    public string ItemNote { get; set; } = string.Empty;

    [ForeignKey("Order")]
    public int OrderId { get; set; }
    public virtual Order? Order { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }
}
