using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.Mock;

public class MockUserRepository : IUserRepository
{
    private static readonly IReadOnlyList<User> Users = new List<User>
    {
        new()
        {
            Id = 1,
            FirstName = "Matej",
            LastName = "Novak",
            Username = "matej.n",
            Email = "matej.n@example.com",
            PasswordHash = "hash_1",
            CreatedAt = new DateTime(2026, 3, 10),
            IsActive = true,
            RoleId = 1
        },
        new()
        {
            Id = 2,
            FirstName = "Ana",
            LastName = "Kovac",
            Username = "ana.k",
            Email = "ana.k@example.com",
            PasswordHash = "hash_2",
            CreatedAt = new DateTime(2026, 3, 11),
            IsActive = true,
            RoleId = 2
        },
        new()
        {
            Id = 3,
            FirstName = "Luka",
            LastName = "Horvat",
            Username = "luka.h",
            Email = "luka.h@example.com",
            PasswordHash = "hash_3",
            CreatedAt = new DateTime(2026, 3, 12),
            IsActive = true,
            RoleId = 2
        }
    };

    public IReadOnlyList<User> GetAll() => Users;

    public User? GetById(int id) => Users.SingleOrDefault(u => u.Id == id);
}
