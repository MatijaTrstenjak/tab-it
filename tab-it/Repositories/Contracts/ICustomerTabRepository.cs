using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface ICustomerTabRepository
{
    IReadOnlyList<CustomerTab> GetAll();
    IReadOnlyList<CustomerTab> GetAllBasic();
    IReadOnlyList<CustomerTab> GetAllForPos();
    CustomerTab? GetById(int id);
    void Add(CustomerTab tab);
    void Update(CustomerTab tab);
    void Delete(int id);
}
