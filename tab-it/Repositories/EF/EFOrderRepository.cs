using Microsoft.EntityFrameworkCore;
using tab_it.DAL;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.EF;

public class EFOrderRepository : IOrderRepository
{
    private readonly TabItDbContext _context;

    public EFOrderRepository(TabItDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Order> GetAll()
    {
        return _context.Orders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(o => o.CustomerTab)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .ToList();
    }

    public Order? GetById(int id)
    {
        return _context.Orders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(o => o.CustomerTab)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefault(o => o.Id == id);
    }

    public void Add(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    public void Update(Order order)
    {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var order = _context.Orders.Find(id);
        if (order is null)
        {
            return;
        }

        order.IsDeleted = true;
        order.DeletedAt = DateTime.UtcNow;
        _context.Orders.Update(order);
        _context.SaveChanges();
    }
}
