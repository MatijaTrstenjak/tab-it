using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface IUserRepository
{
    IReadOnlyList<User> GetAll();
    User? GetById(int id);
    void Add(User user);
    void Update(User user);
    void Delete(int id);
}
