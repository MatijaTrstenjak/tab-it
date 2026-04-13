using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface IOrderItemRepository
{
    IReadOnlyList<OrderItem> GetAll();
    OrderItem? GetById(int id);
}
