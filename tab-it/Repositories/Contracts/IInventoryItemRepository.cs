using tab_it.Models.Domain;

namespace tab_it.Repositories.Contracts;

public interface IInventoryItemRepository
{
    IReadOnlyList<InventoryItem> GetAll();
    InventoryItem? GetById(int id);
    void Add(InventoryItem item);
    void Update(InventoryItem item);
    void Delete(int id);
}
