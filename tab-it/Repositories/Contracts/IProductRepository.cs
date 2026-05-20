using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
    Product? GetById(int id);
    void Add(Product product);
    void Update(Product product);
    void Delete(int id);
}
