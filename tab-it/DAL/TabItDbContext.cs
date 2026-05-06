using Microsoft.EntityFrameworkCore;
using tab_it.Models.Domain;

namespace tab_it.DAL;

public class TabItDbContext : DbContext
{
    public TabItDbContext(DbContextOptions<TabItDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CustomerTab> CustomerTabs { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed some initial data for testing
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", Description = "Administrator role" },
            new Role { Id = 2, Name = "Staff", Description = "Staff role" }
        );

        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = 1, 
                FirstName = "Admin", 
                LastName = "User", 
                Username = "admin", 
                Email = "admin@tab-it.local", 
                PasswordHash = "admin", 
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), 
                IsActive = true, 
                RoleId = 1 
            }
        );

        modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory { Id = 1, Name = "Beverages", Description = "Drinks and refreshments" },
            new ProductCategory { Id = 2, Name = "Main Course", Description = "Hot meals and main dishes" },
            new ProductCategory { Id = 3, Name = "Desserts", Description = "Sweet treats" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Cola", UnitPrice = 2.50m, AvailableQuantity = 100, Sku = "BEV-001", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 2, Name = "Coffee",  UnitPrice = 3.00m, AvailableQuantity = 50, Sku = "BEV-002", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 3, Name = "Cheeseburger", UnitPrice = 12.99m, AvailableQuantity = 20, Sku = "MAI-001", ProductCategoryId = 2, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 4, Name = "Pepperoni Pizza", UnitPrice = 15.50m, AvailableQuantity = 15, Sku = "MAI-002", ProductCategoryId = 2, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 5, Name = "Chocolate Cake", UnitPrice = 6.00m, AvailableQuantity = 10, Sku = "DES-001", ProductCategoryId = 3, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<CustomerTab>().HasData(
            new CustomerTab { Id = 1, TabCode = "TAB-001", TableNumber = 5, OpenedAt = new DateTime(2026, 5, 6, 12, 0, 0, DateTimeKind.Utc), Status = TabStatus.Open, OpenedByUserId = 1 },
            new CustomerTab { Id = 2, TabCode = "TAB-002", TableNumber = 12, OpenedAt = new DateTime(2026, 5, 6, 10, 0, 0, DateTimeKind.Utc), ClosedAt = new DateTime(2026, 5, 6, 11, 30, 0, DateTimeKind.Utc), Status = TabStatus.Closed, OpenedByUserId = 1 }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, OrderNumber = "ORD-001", Subtotal = 15.49m, DiscountPercent = 0, Total = 15.49m, CustomerTabId = 1, Status = OrderStatus.Served },
            new Order { Id = 2, OrderNumber = "ORD-002", Subtotal = 18.50m, DiscountPercent = 10, Total = 16.65m, CustomerTabId = 2, Status = OrderStatus.Served }
        );

        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { Id = 1, OrderId = 1, ProductId = 3, Quantity = 1, UnitPrice = 12.99m, LineTotal = 12.99m, ItemNote = "No onions" },
            new OrderItem { Id = 2, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 2.50m, LineTotal = 2.50m },
            new OrderItem { Id = 3, OrderId = 2, ProductId = 4, Quantity = 1, UnitPrice = 15.50m, LineTotal = 15.50m },
            new OrderItem { Id = 4, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 3.00m, LineTotal = 3.00m }
        );
    }
}