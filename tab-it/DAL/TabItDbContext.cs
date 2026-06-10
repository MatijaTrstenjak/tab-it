using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using tab_it.Models.Domain;

namespace tab_it.DAL;

public class TabItDbContext : IdentityDbContext<AppUser>
{
    public TabItDbContext(DbContextOptions<TabItDbContext> options) : base(options)
    {
    }

    public DbSet<Role> StaffRoles { get; set; }
    public DbSet<User> StaffProfiles { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CustomerTab> CustomerTabs { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<ProductRecipeItem> ProductRecipeItems { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<ProductImage>().ToTable("ProductImages");

        modelBuilder.Entity<AppUser>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<ProductCategory>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<CustomerTab>().HasQueryFilter(t => !t.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
        modelBuilder.Entity<OrderItem>().HasQueryFilter(i => !i.IsDeleted);
        modelBuilder.Entity<InventoryItem>().HasQueryFilter(i => !i.IsDeleted);
        modelBuilder.Entity<ProductRecipeItem>().HasQueryFilter(i => !i.IsDeleted);
        modelBuilder.Entity<ProductImage>().HasQueryFilter(i => !i.IsDeleted);

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
            new Product { Id = 1, Name = "Coca Cola 0.33L", UnitPrice = 2.50m, AvailableQuantity = 100, Sku = "BEV-001", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 2, Name = "Coffee with Milk",  UnitPrice = 3.00m, AvailableQuantity = 50, Sku = "BEV-002", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 3, Name = "Cheeseburger", UnitPrice = 12.99m, AvailableQuantity = 20, Sku = "MAI-001", ProductCategoryId = 2, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 4, Name = "Pepperoni Pizza", UnitPrice = 15.50m, AvailableQuantity = 15, Sku = "MAI-002", ProductCategoryId = 2, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 5, Name = "Chocolate Cake", UnitPrice = 6.00m, AvailableQuantity = 10, Sku = "DES-001", ProductCategoryId = 3, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 6, Name = "Espresso", UnitPrice = 2.20m, AvailableQuantity = 0, Sku = "BEV-003", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 7, Name = "Cappuccino", UnitPrice = 3.40m, AvailableQuantity = 0, Sku = "BEV-004", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 8, Name = "Latte", UnitPrice = 3.60m, AvailableQuantity = 0, Sku = "BEV-005", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 9, Name = "Fanta 0.33L", UnitPrice = 2.50m, AvailableQuantity = 0, Sku = "BEV-006", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 10, Name = "Sprite 0.33L", UnitPrice = 2.50m, AvailableQuantity = 0, Sku = "BEV-007", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 11, Name = "Still Water 0.5L", UnitPrice = 1.80m, AvailableQuantity = 0, Sku = "BEV-008", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 12, Name = "Orange Juice", UnitPrice = 3.20m, AvailableQuantity = 0, Sku = "BEV-009", ProductCategoryId = 1, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 13, Name = "Tiramisu", UnitPrice = 5.50m, AvailableQuantity = 0, Sku = "DES-002", ProductCategoryId = 3, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 14, Name = "Cheesecake", UnitPrice = 5.80m, AvailableQuantity = 0, Sku = "DES-003", ProductCategoryId = 3, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 15, Name = "Apple Pie", UnitPrice = 4.80m, AvailableQuantity = 0, Sku = "DES-004", ProductCategoryId = 3, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 16, Name = "Croissant", UnitPrice = 2.70m, AvailableQuantity = 0, Sku = "DES-005", ProductCategoryId = 3, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 17, Name = "Blueberry Muffin", UnitPrice = 3.10m, AvailableQuantity = 0, Sku = "DES-006", ProductCategoryId = 3, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 18, Name = "Club Sandwich", UnitPrice = 8.90m, AvailableQuantity = 0, Sku = "MAI-003", ProductCategoryId = 2, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 19, Name = "Caesar Salad", UnitPrice = 7.80m, AvailableQuantity = 0, Sku = "MAI-004", ProductCategoryId = 2, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 20, Name = "Brownie", UnitPrice = 3.90m, AvailableQuantity = 0, Sku = "DES-007", ProductCategoryId = 3, IsAlcoholic = false, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<InventoryItem>().HasData(
            new InventoryItem { Id = 1, Name = "Coffee Beans", Sku = "INV-COF-BEAN", Unit = InventoryUnit.Kilogram, QuantityOnHand = 5.000m, ReorderLevel = 1.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 2, Name = "Milk", Sku = "INV-MILK", Unit = InventoryUnit.Liter, QuantityOnHand = 12.000m, ReorderLevel = 2.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 3, Name = "Coca Cola 0.33L Can", Sku = "INV-COLA-033", Unit = InventoryUnit.Quantity, QuantityOnHand = 100.000m, ReorderLevel = 12.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 4, Name = "Fanta 0.33L Can", Sku = "INV-FANTA-033", Unit = InventoryUnit.Quantity, QuantityOnHand = 80.000m, ReorderLevel = 12.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 5, Name = "Sprite 0.33L Can", Sku = "INV-SPRITE-033", Unit = InventoryUnit.Quantity, QuantityOnHand = 80.000m, ReorderLevel = 12.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 6, Name = "Still Water 0.5L Bottle", Sku = "INV-WATER-050", Unit = InventoryUnit.Quantity, QuantityOnHand = 90.000m, ReorderLevel = 18.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 7, Name = "Orange Juice", Sku = "INV-ORANGE-JUICE", Unit = InventoryUnit.Liter, QuantityOnHand = 10.000m, ReorderLevel = 2.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 8, Name = "Chocolate Cake Slice", Sku = "INV-CAKE-CHOC", Unit = InventoryUnit.Quantity, QuantityOnHand = 18.000m, ReorderLevel = 4.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 9, Name = "Tiramisu Slice", Sku = "INV-TIRAMISU", Unit = InventoryUnit.Quantity, QuantityOnHand = 16.000m, ReorderLevel = 4.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 10, Name = "Cheesecake Slice", Sku = "INV-CHEESECAKE", Unit = InventoryUnit.Quantity, QuantityOnHand = 16.000m, ReorderLevel = 4.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 11, Name = "Apple Pie Slice", Sku = "INV-APPLE-PIE", Unit = InventoryUnit.Quantity, QuantityOnHand = 14.000m, ReorderLevel = 4.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 12, Name = "Croissant", Sku = "INV-CROISSANT", Unit = InventoryUnit.Quantity, QuantityOnHand = 24.000m, ReorderLevel = 6.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 13, Name = "Blueberry Muffin", Sku = "INV-MUFFIN-BLUE", Unit = InventoryUnit.Quantity, QuantityOnHand = 20.000m, ReorderLevel = 5.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 14, Name = "Brownie", Sku = "INV-BROWNIE", Unit = InventoryUnit.Quantity, QuantityOnHand = 20.000m, ReorderLevel = 5.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 15, Name = "Burger Patty", Sku = "INV-BURGER-PATTY", Unit = InventoryUnit.Quantity, QuantityOnHand = 30.000m, ReorderLevel = 8.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 16, Name = "Burger Bun", Sku = "INV-BURGER-BUN", Unit = InventoryUnit.Quantity, QuantityOnHand = 30.000m, ReorderLevel = 8.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 17, Name = "Pizza Dough", Sku = "INV-PIZZA-DOUGH", Unit = InventoryUnit.Quantity, QuantityOnHand = 25.000m, ReorderLevel = 6.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 18, Name = "Pepperoni", Sku = "INV-PEPPERONI", Unit = InventoryUnit.Kilogram, QuantityOnHand = 3.000m, ReorderLevel = 0.500m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 19, Name = "Cheese", Sku = "INV-CHEESE", Unit = InventoryUnit.Kilogram, QuantityOnHand = 5.000m, ReorderLevel = 1.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 20, Name = "Sandwich Bread", Sku = "INV-SAND-BREAD", Unit = InventoryUnit.Quantity, QuantityOnHand = 60.000m, ReorderLevel = 12.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 21, Name = "Chicken", Sku = "INV-CHICKEN", Unit = InventoryUnit.Kilogram, QuantityOnHand = 6.000m, ReorderLevel = 1.000m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 22, Name = "Lettuce", Sku = "INV-LETTUCE", Unit = InventoryUnit.Kilogram, QuantityOnHand = 3.000m, ReorderLevel = 0.700m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new InventoryItem { Id = 23, Name = "Caesar Dressing", Sku = "INV-CAESAR-DRESS", Unit = InventoryUnit.Liter, QuantityOnHand = 4.000m, ReorderLevel = 0.750m, LastRestockedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<ProductRecipeItem>().HasData(
            new ProductRecipeItem { Id = 1, ProductId = 1, InventoryItemId = 3, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 2, ProductId = 2, InventoryItemId = 1, QuantityRequired = 0.012m },
            new ProductRecipeItem { Id = 3, ProductId = 2, InventoryItemId = 2, QuantityRequired = 0.100m },
            new ProductRecipeItem { Id = 4, ProductId = 3, InventoryItemId = 15, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 5, ProductId = 3, InventoryItemId = 16, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 6, ProductId = 4, InventoryItemId = 17, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 7, ProductId = 4, InventoryItemId = 18, QuantityRequired = 0.050m },
            new ProductRecipeItem { Id = 8, ProductId = 4, InventoryItemId = 19, QuantityRequired = 0.120m },
            new ProductRecipeItem { Id = 9, ProductId = 5, InventoryItemId = 8, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 10, ProductId = 6, InventoryItemId = 1, QuantityRequired = 0.009m },
            new ProductRecipeItem { Id = 11, ProductId = 7, InventoryItemId = 1, QuantityRequired = 0.012m },
            new ProductRecipeItem { Id = 12, ProductId = 7, InventoryItemId = 2, QuantityRequired = 0.100m },
            new ProductRecipeItem { Id = 13, ProductId = 8, InventoryItemId = 1, QuantityRequired = 0.012m },
            new ProductRecipeItem { Id = 14, ProductId = 8, InventoryItemId = 2, QuantityRequired = 0.180m },
            new ProductRecipeItem { Id = 15, ProductId = 9, InventoryItemId = 4, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 16, ProductId = 10, InventoryItemId = 5, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 17, ProductId = 11, InventoryItemId = 6, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 18, ProductId = 12, InventoryItemId = 7, QuantityRequired = 0.250m },
            new ProductRecipeItem { Id = 19, ProductId = 13, InventoryItemId = 9, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 20, ProductId = 14, InventoryItemId = 10, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 21, ProductId = 15, InventoryItemId = 11, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 22, ProductId = 16, InventoryItemId = 12, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 23, ProductId = 17, InventoryItemId = 13, QuantityRequired = 1.000m },
            new ProductRecipeItem { Id = 24, ProductId = 18, InventoryItemId = 20, QuantityRequired = 2.000m },
            new ProductRecipeItem { Id = 25, ProductId = 18, InventoryItemId = 21, QuantityRequired = 0.120m },
            new ProductRecipeItem { Id = 26, ProductId = 18, InventoryItemId = 22, QuantityRequired = 0.030m },
            new ProductRecipeItem { Id = 27, ProductId = 19, InventoryItemId = 21, QuantityRequired = 0.120m },
            new ProductRecipeItem { Id = 28, ProductId = 19, InventoryItemId = 22, QuantityRequired = 0.120m },
            new ProductRecipeItem { Id = 29, ProductId = 19, InventoryItemId = 23, QuantityRequired = 0.030m },
            new ProductRecipeItem { Id = 30, ProductId = 20, InventoryItemId = 14, QuantityRequired = 1.000m }
        );

        modelBuilder.Entity<CustomerTab>().HasData(
            new CustomerTab { Id = 1, TabCode = "TAB-001", TableNumber = 5, OpenedAt = new DateTime(2026, 5, 6, 12, 0, 0, DateTimeKind.Utc), Status = TabStatus.Open, OpenedByUserId = 1 },
            new CustomerTab { Id = 2, TabCode = "TAB-002", TableNumber = 12, OpenedAt = new DateTime(2026, 5, 6, 10, 0, 0, DateTimeKind.Utc), ClosedAt = new DateTime(2026, 5, 6, 11, 30, 0, DateTimeKind.Utc), Status = TabStatus.Closed, OpenedByUserId = 1, PaymentMethod = "Cash" }
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
