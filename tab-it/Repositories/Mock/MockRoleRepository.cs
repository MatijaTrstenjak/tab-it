using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.Mock;

public class MockRoleRepository : IRoleRepository
{
    private static readonly IReadOnlyList<Role> Roles = new List<Role>
    {
        new()
        {
            Id = 1,
            Name = "Admin",
            Description = "Can manage users, products and all tabs."
        },
        new()
        {
            Id = 2,
            Name = "Waiter",
            Description = "Opens tabs and serves customers."
        }
    };

    public IReadOnlyList<Role> GetAll() => Roles;

    public Role? GetById(int id) => Roles.SingleOrDefault(r => r.Id == id);
}
