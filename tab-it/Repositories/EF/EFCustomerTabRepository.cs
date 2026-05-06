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
        return _context.CustomerTabs.Include(t => t.OpenedByUser).ToList();
    }

    public CustomerTab? GetById(int id)
    {
        return _context.CustomerTabs.Include(t => t.OpenedByUser).FirstOrDefault(t => t.Id == id);
    }

    public void Add(CustomerTab tab)
    {
        _context.CustomerTabs.Add(tab);
        _context.SaveChanges();
    }
}