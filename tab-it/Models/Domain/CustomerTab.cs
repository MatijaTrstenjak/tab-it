using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tab_it.Models.Domain;

public class CustomerTab
{
    [Key]
    public int Id { get; set; }
    public string TabCode { get; set; } = string.Empty;
    public int TableNumber { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public TabStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;

    [ForeignKey("OpenedByUser")]
    public int OpenedByUserId { get; set; }
    public virtual User? OpenedByUser { get; set; }

    // 1-N: one tab can have many orders.
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
