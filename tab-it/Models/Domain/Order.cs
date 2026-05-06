using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class Order
{
    [Key]
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderedAt { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal Total { get; set; }

    [ForeignKey("CustomerTab")]
    public int CustomerTabId { get; set; }
    public virtual CustomerTab? CustomerTab { get; set; }

    // N-N with Product is represented via OrderItem bridge class.
    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
