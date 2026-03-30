namespace tab_it.Models.Domain;

public class ProductCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // 1-N: one category can contain many products.
    public List<Product> Products { get; set; } = new();
}
