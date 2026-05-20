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
        return _context.Products.Include(p => p.Category).ToList();
    }

    public Product? GetById(int id)
    {
        return _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
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

        _context.Products.Remove(product);
        _context.SaveChanges();
    }
}