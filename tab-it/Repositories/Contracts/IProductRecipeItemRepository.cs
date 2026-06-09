using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface IProductRecipeItemRepository
{
    IReadOnlyList<ProductRecipeItem> GetAll();
    IReadOnlyList<ProductRecipeItem> GetByProductId(int productId);
    ProductRecipeItem? GetById(int id);
    void Add(ProductRecipeItem item);
    void Update(ProductRecipeItem item);
    void Delete(int id);
}
