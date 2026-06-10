using Microsoft.EntityFrameworkCore;
using tab_it.DAL;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.EF;

public class EFProductRepository : IProductRepository
{
    private readonly TabItDbContext _context;

    public EFProductRepository(TabItDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Product> GetAll()
    {
        return _context.Products
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.RecipeItems)
                .ThenInclude(r => r.InventoryItem)
            .ToList();
    }

    public Product? GetById(int id)
    {
        return _context.Products
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.RecipeItems)
                .ThenInclude(r => r.InventoryItem)
            .FirstOrDefault(p => p.Id == id);
    }

    public void Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product is null)
        {
            return;
        }

        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;
        _context.Products.Update(product);
        _context.SaveChanges();
    }
}
