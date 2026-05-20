using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface IRoleRepository
{
    IReadOnlyList<Role> GetAll();
    Role? GetById(int id);
    void Add(Role role);
    void Update(Role role);
    void Delete(int id);
}
