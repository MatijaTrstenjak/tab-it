using tab_it.DAL;
using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;

namespace tab_it.Repositories.EF;

public class EFProductCategoryRepository : IProductCategoryRepository
{
    private readonly TabItDbContext _context;

    public EFProductCategoryRepository(TabItDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<ProductCategory> GetAll()
    {
        return _context.ProductCategories.ToList();
    }

    public ProductCategory? GetById(int id)
    {
        return _context.ProductCategories.Find(id);
    }

    public void Add(ProductCategory category)
    {
        _context.ProductCategories.Add(category);
        _context.SaveChanges();
    }

    public void Update(ProductCategory category)
    {
        _context.ProductCategories.Update(category);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var category = _context.ProductCategories.Find(id);
        if (category is null)
        {
            return;
        }

        _context.ProductCategories.Remove(category);
        _context.SaveChanges();
    }
}