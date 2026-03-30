using tab_it.Models.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Temporary in-memory data for Lab-1 model work.
var adminRole = new Role
{
    Id = 1,
    Name = "Admin",
    Description = "Can manage users, products and all tabs."
};

var waiterRole = new Role
{
    Id = 2,
    Name = "Waiter",
    Description = "Opens tabs and serves customers."
};

var users = new List<User>
{
    new()
    {
        Id = 1,
        FirstName = "Matej",
        LastName = "Novak",
        Username = "matej.n",
        Email = "matej.n@example.com",
        PasswordHash = "hash_1",
        CreatedAt = new DateTime(2026, 3, 10),
        IsActive = true,
        RoleId = adminRole.Id,
        Role = adminRole
    },
    new()
    {
        Id = 2,
        FirstName = "Ana",
        LastName = "Kovac",
        Username = "ana.k",
        Email = "ana.k@example.com",
        PasswordHash = "hash_2",
        CreatedAt = new DateTime(2026, 3, 11),
        IsActive = true,
        RoleId = waiterRole.Id,
        Role = waiterRole
    },
    new()
    {
        Id = 3,
        FirstName = "Luka",
        LastName = "Horvat",
        Username = "luka.h",
        Email = "luka.h@example.com",
        PasswordHash = "hash_3",
        CreatedAt = new DateTime(2026, 3, 12),
        IsActive = true,
        RoleId = waiterRole.Id,
        Role = waiterRole
    }
};

adminRole.Users.Add(users[0]);
waiterRole.Users.Add(users[1]);
waiterRole.Users.Add(users[2]);

var drinkCategory = new ProductCategory
{
    Id = 1,
    Name = "Drinks",
    Description = "Hot and cold drinks"
};

var foodCategory = new ProductCategory
{
    Id = 2,
    Name = "Food",
    Description = "Main and snack dishes"
};

var products = new List<Product>
{
    new()
    {
        Id = 1,
        Name = "Espresso",
        Sku = "DRINK-ESP-001",
        UnitPrice = 1.80m,
        IsAlcoholic = false,
        AvailableQuantity = 200,
        LastRestockedAt = new DateTime(2026, 3, 28),
        ProductCategoryId = drinkCategory.Id,
        Category = drinkCategory
    },
    new()
    {
        Id = 2,
        Name = "Fresh Orange Juice",
        Sku = "DRINK-OJ-002",
        UnitPrice = 3.20m,
        IsAlcoholic = false,
        AvailableQuantity = 80,
        LastRestockedAt = new DateTime(2026, 3, 29),
        ProductCategoryId = drinkCategory.Id,
        Category = drinkCategory
    },
    new()
    {
        Id = 3,
        Name = "Club Sandwich",
        Sku = "FOOD-SND-003",
        UnitPrice = 5.50m,
        IsAlcoholic = false,
        AvailableQuantity = 35,
        LastRestockedAt = new DateTime(2026, 3, 27),
        ProductCategoryId = foodCategory.Id,
        Category = foodCategory
    }
};

drinkCategory.Products.Add(products[0]);
drinkCategory.Products.Add(products[1]);
foodCategory.Products.Add(products[2]);

var tabs = new List<CustomerTab>
{
    new()
    {
        Id = 1,
        TabCode = "TAB-001",
        TableNumber = 7,
        OpenedAt = new DateTime(2026, 3, 30, 18, 10, 0),
        Status = TabStatus.Open,
        Notes = "Birthday group",
        OpenedByUserId = users[1].Id,
        OpenedByUser = users[1]
    },
    new()
    {
        Id = 2,
        TabCode = "TAB-002",
        TableNumber = 3,
        OpenedAt = new DateTime(2026, 3, 30, 18, 25, 0),
        Status = TabStatus.Open,
        Notes = "Allergic to nuts",
        OpenedByUserId = users[2].Id,
        OpenedByUser = users[2]
    },
    new()
    {
        Id = 3,
        TabCode = "TAB-003",
        TableNumber = 12,
        OpenedAt = new DateTime(2026, 3, 30, 17, 40, 0),
        ClosedAt = new DateTime(2026, 3, 30, 19, 00, 0),
        Status = TabStatus.Closed,
        Notes = "Paid by card",
        OpenedByUserId = users[1].Id,
        OpenedByUser = users[1]
    }
};

users[1].Tabs.Add(tabs[0]);
users[1].Tabs.Add(tabs[2]);
users[2].Tabs.Add(tabs[1]);

var orders = new List<Order>
{
    new()
    {
        Id = 1,
        OrderNumber = "ORD-001",
        OrderedAt = new DateTime(2026, 3, 30, 18, 15, 0),
        Status = OrderStatus.Served,
        Subtotal = 7.30m,
        DiscountPercent = 0m,
        Total = 7.30m,
        CustomerTabId = tabs[0].Id,
        CustomerTab = tabs[0]
    },
    new()
    {
        Id = 2,
        OrderNumber = "ORD-002",
        OrderedAt = new DateTime(2026, 3, 30, 18, 35, 0),
        Status = OrderStatus.Ready,
        Subtotal = 11.00m,
        DiscountPercent = 10m,
        Total = 9.90m,
        CustomerTabId = tabs[1].Id,
        CustomerTab = tabs[1]
    },
    new()
    {
        Id = 3,
        OrderNumber = "ORD-003",
        OrderedAt = new DateTime(2026, 3, 30, 17, 55, 0),
        Status = OrderStatus.Served,
        Subtotal = 5.50m,
        DiscountPercent = 0m,
        Total = 5.50m,
        CustomerTabId = tabs[2].Id,
        CustomerTab = tabs[2]
    }
};

tabs[0].Orders.Add(orders[0]);
tabs[1].Orders.Add(orders[1]);
tabs[2].Orders.Add(orders[2]);

var orderItems = new List<OrderItem>
{
    new()
    {
        Id = 1,
        OrderId = orders[0].Id,
        Order = orders[0],
        ProductId = products[0].Id,
        Product = products[0],
        Quantity = 1,
        UnitPrice = 1.80m,
        LineTotal = 1.80m,
        ItemNote = "No sugar"
    },
    new()
    {
        Id = 2,
        OrderId = orders[0].Id,
        Order = orders[0],
        ProductId = products[2].Id,
        Product = products[2],
        Quantity = 1,
        UnitPrice = 5.50m,
        LineTotal = 5.50m,
        ItemNote = "Extra sauce"
    },
    new()
    {
        Id = 3,
        OrderId = orders[1].Id,
        Order = orders[1],
        ProductId = products[1].Id,
        Product = products[1],
        Quantity = 2,
        UnitPrice = 3.20m,
        LineTotal = 6.40m,
        ItemNote = "With ice"
    },
    new()
    {
        Id = 4,
        OrderId = orders[2].Id,
        Order = orders[2],
        ProductId = products[2].Id,
        Product = products[2],
        Quantity = 1,
        UnitPrice = 5.50m,
        LineTotal = 5.50m,
        ItemNote = "No onions"
    }
};

orders[0].Items.Add(orderItems[0]);
orders[0].Items.Add(orderItems[1]);
orders[1].Items.Add(orderItems[2]);
orders[2].Items.Add(orderItems[3]);

products[0].OrderItems.Add(orderItems[0]);
products[1].OrderItems.Add(orderItems[2]);
products[2].OrderItems.Add(orderItems[1]);
products[2].OrderItems.Add(orderItems[3]);

Console.WriteLine("=== Lab-1 LINQ demo over in-memory model ===");

Console.WriteLine("\n1) Filter: all currently open tabs (Where)");
var openTabs = tabs
    .Where(t => t.Status == TabStatus.Open)
    .ToList();
foreach (var tab in openTabs)
{
    Console.WriteLine($"   - {tab.TabCode} | table {tab.TableNumber} | opened by {tab.OpenedByUser?.Username}");
}

Console.WriteLine("\n2) Sort: products by price descending (OrderByDescending)");
var productsByPrice = products
    .OrderByDescending(p => p.UnitPrice)
    .ToList();
foreach (var product in productsByPrice)
{
    Console.WriteLine($"   - {product.Name}: {product.UnitPrice:F2} EUR");
}

Console.WriteLine("\n3) Aggregate: total value of all served orders (Where + Sum)");
var servedOrdersTotal = orders
    .Where(o => o.Status == OrderStatus.Served)
    .Sum(o => o.Total);
Console.WriteLine($"   - Served orders total: {servedOrdersTotal:F2} EUR");

Console.WriteLine("\n4) Count: number of orders that have a discount (Count)");
var discountedOrdersCount = orders.Count(o => o.DiscountPercent > 0);
Console.WriteLine($"   - Discounted orders count: {discountedOrdersCount}");

Console.WriteLine("\n5) Single element: find the admin user (Single)");
var adminUser = users.Single(u => u.Role?.Name == "Admin");
Console.WriteLine($"   - Admin is: {adminUser.FirstName} {adminUser.LastName} ({adminUser.Username})");

Console.WriteLine("\n6) FirstOrDefault: first closed tab ordered by opening time");
var firstClosedTab = tabs
    .Where(t => t.Status == TabStatus.Closed)
    .OrderBy(t => t.OpenedAt)
    .FirstOrDefault();
Console.WriteLine(firstClosedTab is null
    ? "   - No closed tabs found"
    : $"   - First closed tab: {firstClosedTab.TabCode} (table {firstClosedTab.TableNumber})");

Console.WriteLine("\n7) Subquery: tabs that contain at least one served order (Any)");
var tabsWithServedOrders = tabs
    .Where(t => t.Orders.Any(o => o.Status == OrderStatus.Served))
    .ToList();
foreach (var tab in tabsWithServedOrders)
{
    Console.WriteLine($"   - {tab.TabCode}");
}

Console.WriteLine("\n8) Grouping: quantity sold per product (GroupBy + Sum)");
var soldByProduct = orderItems
    .GroupBy(oi => oi.Product?.Name ?? "Unknown")
    .Select(g => new
    {
        ProductName = g.Key,
        QuantitySold = g.Sum(x => x.Quantity),
        Revenue = g.Sum(x => x.LineTotal)
    })
    .OrderByDescending(x => x.QuantitySold)
    .ThenByDescending(x => x.Revenue)
    .ToList();
foreach (var item in soldByProduct)
{
    Console.WriteLine($"   - {item.ProductName}: qty {item.QuantitySold}, revenue {item.Revenue:F2} EUR");
}

Console.WriteLine("\n9) Projection: compact order report (Select)");
var orderReport = orders
    .Select(o => new
    {
        o.OrderNumber,
        TabCode = o.CustomerTab?.TabCode ?? "N/A",
        ItemCount = o.Items.Sum(i => i.Quantity),
        o.Total,
        o.Status
    })
    .OrderBy(x => x.OrderNumber)
    .ToList();
foreach (var row in orderReport)
{
    Console.WriteLine($"   - {row.OrderNumber} | tab {row.TabCode} | items {row.ItemCount} | total {row.Total:F2} EUR | {row.Status}");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Logger.LogInformation(
    "Seeded {Users} users, {Products} products, {Tabs} tabs, {Orders} orders.",
    users.Count,
    products.Count,
    tabs.Count,
    orders.Count);


app.Run();
