using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface IRoleRepository
{
    IReadOnlyList<Role> GetAll();
    Role? GetById(int id);
}
