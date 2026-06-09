using tab_it.Models.Domain;

namespace tab_it.Models
{
    public class DashboardOrderCardViewModel
    {
        public DashboardOrderCardViewModel(
            Order order,
            string statusClass,
            string statusLabel,
            OrderStatus? nextStatus,
            string? actionLabel)
        {
            Order = order;
            StatusClass = statusClass;
            StatusLabel = statusLabel;
            NextStatus = nextStatus;
            ActionLabel = actionLabel;
        }

        public Order Order { get; }
        public string StatusClass { get; }
        public string StatusLabel { get; }
        public OrderStatus? NextStatus { get; }
        public string? ActionLabel { get; }
    }
}
