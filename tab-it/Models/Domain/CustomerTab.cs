using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class CustomerTab : ISoftDeletable
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(40)]
    public string TabCode { get; set; } = string.Empty;

    [Range(1, 999)]
    public int TableNumber { get; set; }

    [Required]
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public TabStatus Status { get; set; }
    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;
    [StringLength(40)]
    public string PaymentMethod { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("OpenedByUser")]
    [Range(1, int.MaxValue)]
    public int OpenedByUserId { get; set; }
    public virtual User? OpenedByUser { get; set; }

    // 1-N: one tab can have many orders.
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
