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
        return _context.StaffRoles.ToList();
    }

    public Role? GetById(int id)
    {
        return _context.StaffRoles.Find(id);
    }

    public void Add(Role role)
    {
        _context.StaffRoles.Add(role);
        _context.SaveChanges();
    }

    public void Update(Role role)
    {
        _context.StaffRoles.Update(role);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var role = _context.StaffRoles.Find(id);
        if (role is null)
        {
            return;
        }

        role.IsDeleted = true;
        role.DeletedAt = DateTime.UtcNow;
        _context.StaffRoles.Update(role);
        _context.SaveChanges();
    }
}
