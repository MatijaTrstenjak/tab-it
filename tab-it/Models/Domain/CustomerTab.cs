namespace tab_it.Models.Domain;

public class CustomerTab
{
    public int Id { get; set; }
    public string TabCode { get; set; } = string.Empty;
    public int TableNumber { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public TabStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;

    public int OpenedByUserId { get; set; }
    public User? OpenedByUser { get; set; }

    // 1-N: one tab can have many orders.
    public List<Order> Orders { get; set; } = new();
}
