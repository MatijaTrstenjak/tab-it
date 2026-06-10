using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class Order : ISoftDeletable
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(40)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    public DateTime OrderedAt { get; set; }
    public OrderStatus Status { get; set; }
    [Range(0, 999999)]
    public decimal Subtotal { get; set; }

    [Range(0, 100)]
    public decimal DiscountPercent { get; set; }

    [Range(0, 999999)]
    public decimal Total { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CustomerTab")]
    [Range(1, int.MaxValue)]
    public int CustomerTabId { get; set; }
    public virtual CustomerTab? CustomerTab { get; set; }

    // N-N with Product is represented via OrderItem bridge class.
    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
