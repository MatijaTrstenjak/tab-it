using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface IProductCategoryRepository
{
    IReadOnlyList<ProductCategory> GetAll();
    ProductCategory? GetById(int id);
}
