using System.ComponentModel.DataAnnotations;

namespace tab_it.Models.Domain;

public class ProductCategory : ISoftDeletable
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // 1-N: one category can contain many products.
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
