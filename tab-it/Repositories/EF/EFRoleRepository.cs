using tab_it.DAL;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.EF;

public class EFRoleRepository : IRoleRepository
{
    private readonly TabItDbContext _context;

    public EFRoleRepository(TabItDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Role> GetAll()
    {
        return _context.Roles.ToList();
    }

    public Role? GetById(int id)
    {
        return _context.Roles.Find(id);
    }

    public void Add(Role role)
    {
        _context.Roles.Add(role);
        _context.SaveChanges();
    }

    public void Update(Role role)
    {
        _context.Roles.Update(role);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var role = _context.Roles.Find(id);
        if (role is null)
        {
            return;
        }

        _context.Roles.Remove(role);
        _context.SaveChanges();
    }
}