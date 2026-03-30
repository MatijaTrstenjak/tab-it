namespace tab_it.Models.Domain;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderedAt { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal Total { get; set; }

    public int CustomerTabId { get; set; }
    public CustomerTab? CustomerTab { get; set; }

    // N-N with Product is represented via OrderItem bridge class.
    public List<OrderItem> Items { get; set; } = new();
}
