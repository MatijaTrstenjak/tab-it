using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface ICustomerTabRepository
{
    IReadOnlyList<CustomerTab> GetAll();
    CustomerTab? GetById(int id);
}
