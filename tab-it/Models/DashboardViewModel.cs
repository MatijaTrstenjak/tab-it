using System.Collections.Generic;
using tab_it.Models.Domain;

namespace tab_it.Models
{
    public class DashboardViewModel
    {
        public int NewOrdersCount { get; set; }
        public int TotalOrdersCount { get; set; }
        public int WaitingListCount { get; set; }
        
        public List<Order> IncomingOrders { get; set; } = new List<Order>();
        public List<Order> OrdersInMaking { get; set; } = new List<Order>();
        public List<Order> OrdersReady { get; set; } = new List<Order>();
        public List<Order> OrdersServed { get; set; } = new List<Order>();
        public List<CustomerTab> OpenTabs { get; set; } = new List<CustomerTab>();
    }
}
