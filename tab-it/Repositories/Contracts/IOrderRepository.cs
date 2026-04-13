using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface IOrderRepository
{
    IReadOnlyList<Order> GetAll();
    Order? GetById(int id);
}
