using Microsoft.EntityFrameworkCore;
using tab_it.DAL;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.EF;

public class EFInventoryItemRepository : IInventoryItemRepository
{
    private readonly TabItDbContext _context;

    public EFInventoryItemRepository(TabItDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<InventoryItem> GetAll()
    {
        return _context.InventoryItems
            .Include(i => i.RecipeItems)
                .ThenInclude(r => r.Product)
            .ToList();
    }

    public InventoryItem? GetById(int id)
    {
        return _context.InventoryItems
            .Include(i => i.RecipeItems)
                .ThenInclude(r => r.Product)
            .FirstOrDefault(i => i.Id == id);
    }

    public void Add(InventoryItem item)
    {
        _context.InventoryItems.Add(item);
        _context.SaveChanges();
    }

    public void Update(InventoryItem item)
    {
        _context.InventoryItems.Update(item);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var item = _context.InventoryItems.Find(id);
        if (item is null)
        {
            return;
        }

        _context.InventoryItems.Remove(item);
        _context.SaveChanges();
    }
}
