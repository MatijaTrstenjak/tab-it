using Microsoft.EntityFrameworkCore;
using tab_it.DAL;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.EF;

public class EFOrderItemRepository : IOrderItemRepository
{
    private readonly TabItDbContext _context;

    public EFOrderItemRepository(TabItDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<OrderItem> GetAll()
    {
        return _context.OrderItems
            .Include(i => i.Order)
            .Include(i => i.Product)
            .ToList();
    }

    public OrderItem? GetById(int id)
    {
        return _context.OrderItems
            .Include(i => i.Order)
            .Include(i => i.Product)
            .FirstOrDefault(i => i.Id == id);
    }

    public void Add(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        _context.SaveChanges();
    }

    public void Update(OrderItem orderItem)
    {
        _context.OrderItems.Update(orderItem);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var orderItem = _context.OrderItems.Find(id);
        if (orderItem is null)
        {
            return;
        }

        _context.OrderItems.Remove(orderItem);
        _context.SaveChanges();
    }
}