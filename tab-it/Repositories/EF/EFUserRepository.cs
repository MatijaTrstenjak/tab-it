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
        return _context.StaffProfiles.Include(u => u.Role).ToList();
    }

    public User? GetById(int id)
    {
        return _context.StaffProfiles.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
    }

    public void Add(User user)
    {
        _context.StaffProfiles.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.StaffProfiles.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = _context.StaffProfiles.Find(id);
        if (user is null)
        {
            return;
        }

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        _context.StaffProfiles.Update(user);
        _context.SaveChanges();
    }
}
