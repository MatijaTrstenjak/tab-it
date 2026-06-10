using tab_it.Models.Domain;

namespace tab_it.Models;

public class AccountingViewModel
{
    public DateTime Today { get; set; }
    public decimal TodaySales { get; set; }
    public int TodayReceipts { get; set; }
    public decimal SevenDaySales { get; set; }
    public IReadOnlyList<PaymentBreakdownViewModel> PaymentBreakdown { get; set; } = new List<PaymentBreakdownViewModel>();
    public IReadOnlyList<DailySalesViewModel> LastSevenDays { get; set; } = new List<DailySalesViewModel>();
    public IReadOnlyList<ReceiptHistoryItemViewModel> Receipts { get; set; } = new List<ReceiptHistoryItemViewModel>();
}

public class PaymentBreakdownViewModel
{
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public int Count { get; set; }
}

public class DailySalesViewModel
{
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
}

public class ReceiptHistoryItemViewModel
{
    public int TabId { get; set; }
    public int TableNumber { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public int OrderCount { get; set; }
}

public class ReceiptDetailsViewModel : ReceiptHistoryItemViewModel
{
    public IReadOnlyList<Order> Orders { get; set; } = new List<Order>();
    public IReadOnlyList<string> PaymentOptions { get; set; } = new List<string>();
}
