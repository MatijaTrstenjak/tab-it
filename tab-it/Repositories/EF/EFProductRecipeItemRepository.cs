using Microsoft.EntityFrameworkCore;
using tab_it.DAL;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.EF;

public class EFProductRecipeItemRepository : IProductRecipeItemRepository
{
    private readonly TabItDbContext _context;

    public EFProductRecipeItemRepository(TabItDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<ProductRecipeItem> GetAll()
    {
        return _context.ProductRecipeItems
            .Include(r => r.Product)
                .ThenInclude(p => p!.Category)
            .Include(r => r.InventoryItem)
            .ToList();
    }

    public IReadOnlyList<ProductRecipeItem> GetByProductId(int productId)
    {
        return _context.ProductRecipeItems
            .Include(r => r.InventoryItem)
            .Where(r => r.ProductId == productId)
            .ToList();
    }

    public ProductRecipeItem? GetById(int id)
    {
        return _context.ProductRecipeItems
            .Include(r => r.Product)
                .ThenInclude(p => p!.Category)
            .Include(r => r.InventoryItem)
            .FirstOrDefault(r => r.Id == id);
    }

    public void Add(ProductRecipeItem item)
    {
        _context.ProductRecipeItems.Add(item);
        _context.SaveChanges();
    }

    public void Update(ProductRecipeItem item)
    {
        _context.ProductRecipeItems.Update(item);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var item = _context.ProductRecipeItems.Find(id);
        if (item is null)
        {
            return;
        }

        _context.ProductRecipeItems.Remove(item);
        _context.SaveChanges();
    }
}
