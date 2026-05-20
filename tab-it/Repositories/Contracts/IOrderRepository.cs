using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface IOrderRepository
{
    IReadOnlyList<Order> GetAll();
    Order? GetById(int id);
    void Add(Order order);
    void Update(Order order);
    void Delete(int id);
}
