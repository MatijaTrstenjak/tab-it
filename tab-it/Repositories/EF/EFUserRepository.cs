using Microsoft.EntityFrameworkCore;
using tab_it.DAL;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.EF;

public class EFUserRepository : IUserRepository
{
    private readonly TabItDbContext _context;

    public EFUserRepository(TabItDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<User> GetAll()
    {
        return _context.Users.Include(u => u.Role).ToList();
    }

    public User? GetById(int id)
    {
        return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user is null)
        {
            return;
        }

        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}