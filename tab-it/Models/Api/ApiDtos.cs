using System.ComponentModel.DataAnnotations;
using tab_it.Models.Domain;

namespace tab_it.Models.Api;

public record ProductCategoryDto(int Id, string Name, string Description);

public class ProductCategoryWriteDto
{
    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;
}

public record ProductDto(int Id, string Name, string Sku, decimal UnitPrice, bool IsAlcoholic, int AvailableQuantity, DateTime LastRestockedAt, int ProductCategoryId);

public class ProductWriteDto
{
    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string Sku { get; set; } = string.Empty;

    [Range(0, 999999)]
    public decimal UnitPrice { get; set; }

    public bool IsAlcoholic { get; set; }

    [Range(0, 999999)]
    public int AvailableQuantity { get; set; }

    public DateTime LastRestockedAt { get; set; } = DateTime.UtcNow;

    [Range(1, int.MaxValue)]
    public int ProductCategoryId { get; set; }
}

public record InventoryItemDto(int Id, string Name, string Sku, InventoryUnit Unit, decimal QuantityOnHand, decimal ReorderLevel, DateTime LastRestockedAt);

public class InventoryItemWriteDto
{
    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string Sku { get; set; } = string.Empty;

    public InventoryUnit Unit { get; set; }

    [Range(0, 999999)]
    public decimal QuantityOnHand { get; set; }

    [Range(0, 999999)]
    public decimal ReorderLevel { get; set; }

    public DateTime LastRestockedAt { get; set; } = DateTime.UtcNow;
}

public record ProductRecipeItemDto(int Id, int ProductId, int InventoryItemId, decimal QuantityRequired);

public class ProductRecipeItemWriteDto
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int InventoryItemId { get; set; }

    [Range(0.0001, 999999)]
    public decimal QuantityRequired { get; set; }
}

public record CustomerTabDto(int Id, string TabCode, int TableNumber, DateTime OpenedAt, DateTime? ClosedAt, TabStatus Status, string Notes, string PaymentMethod, int OpenedByUserId);

public class CustomerTabWriteDto
{
    [Required, StringLength(40)]
    public string TabCode { get; set; } = string.Empty;

    [Range(1, 999)]
    public int TableNumber { get; set; }

    public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }
    public TabStatus Status { get; set; } = TabStatus.Open;

    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;

    [StringLength(40)]
    public string PaymentMethod { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int OpenedByUserId { get; set; }
}

public record OrderDto(int Id, string OrderNumber, DateTime OrderedAt, OrderStatus Status, decimal Subtotal, decimal DiscountPercent, decimal Total, int CustomerTabId);

public class OrderWriteDto
{
    [Required, StringLength(40)]
    public string OrderNumber { get; set; } = string.Empty;

    public DateTime OrderedAt { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Draft;

    [Range(0, 999999)]
    public decimal Subtotal { get; set; }

    [Range(0, 100)]
    public decimal DiscountPercent { get; set; }

    [Range(0, 999999)]
    public decimal Total { get; set; }

    [Range(1, int.MaxValue)]
    public int CustomerTabId { get; set; }
}

public record OrderItemDto(int Id, int Quantity, decimal UnitPrice, decimal LineTotal, string ItemNote, int OrderId, int ProductId);

public class OrderItemWriteDto
{
    [Range(1, 9999)]
    public int Quantity { get; set; }

    [Range(0, 999999)]
    public decimal UnitPrice { get; set; }

    [Range(0, 999999)]
    public decimal LineTotal { get; set; }

    [StringLength(200)]
    public string ItemNote { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }
}

public record StaffUserDto(int Id, string FirstName, string LastName, string Username, string Email, DateTime CreatedAt, bool IsActive, int RoleId);

public class StaffUserWriteDto
{
    [Required, StringLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    [Range(1, int.MaxValue)]
    public int RoleId { get; set; }
}

public record StaffRoleDto(int Id, string Name, string Description);

public class StaffRoleWriteDto
{
    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;
}

public record ProductImageDto(int Id, int ProductId, string OriginalFileName, string StoredFileName, string RelativePath, string ContentType, long FileSize, DateTime CreatedAt);
