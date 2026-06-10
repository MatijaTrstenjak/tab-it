using Microsoft.EntityFrameworkCore;
using tab_it.DAL;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.EF;

public class EFCustomerTabRepository : ICustomerTabRepository
{
    private readonly TabItDbContext _context;

    public EFCustomerTabRepository(TabItDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<CustomerTab> GetAll()
    {
        return _context.CustomerTabs
            .AsNoTracking()
            .AsSplitQuery()
            .Include(t => t.OpenedByUser)
            .Include(t => t.Orders)
                .ThenInclude(o => o.Items)
                    .ThenInclude(i => i.Product)
            .ToList();
    }

    public IReadOnlyList<CustomerTab> GetAllBasic()
    {
        return _context.CustomerTabs
            .AsNoTracking()
            .ToList();
    }

    public IReadOnlyList<CustomerTab> GetAllForPos()
    {
        return _context.CustomerTabs
            .AsNoTracking()
            .AsSplitQuery()
            .Include(t => t.Orders)
            .ToList();
    }

    public CustomerTab? GetById(int id)
    {
        return _context.CustomerTabs
            .AsNoTracking()
            .AsSplitQuery()
            .Include(t => t.OpenedByUser)
            .Include(t => t.Orders)
                .ThenInclude(o => o.Items)
                    .ThenInclude(i => i.Product)
            .FirstOrDefault(t => t.Id == id);
    }

    public void Add(CustomerTab tab)
    {
        _context.CustomerTabs.Add(tab);
        _context.SaveChanges();
    }

    public void Update(CustomerTab tab)
    {
        _context.CustomerTabs.Update(tab);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var tab = _context.CustomerTabs.Find(id);
        if (tab is null)
        {
            return;
        }

        tab.IsDeleted = true;
        tab.DeletedAt = DateTime.UtcNow;
        _context.CustomerTabs.Update(tab);
        _context.SaveChanges();
    }
}
