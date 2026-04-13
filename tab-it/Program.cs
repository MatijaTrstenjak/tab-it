using tab_it.Models.Domain;
using tab_it.Repositories.Contracts;
using tab_it.Repositories.Mock;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IRoleRepository, MockRoleRepository>();
builder.Services.AddSingleton<IUserRepository, MockUserRepository>();
builder.Services.AddSingleton<IProductCategoryRepository, MockProductCategoryRepository>();
builder.Services.AddSingleton<IProductRepository, MockProductRepository>();
builder.Services.AddSingleton<ICustomerTabRepository, MockCustomerTabRepository>();
builder.Services.AddSingleton<IOrderRepository, MockOrderRepository>();
builder.Services.AddSingleton<IOrderItemRepository, MockOrderItemRepository>();

var app = builder.Build();

var roles = app.Services.GetRequiredService<IRoleRepository>().GetAll();
var users = app.Services.GetRequiredService<IUserRepository>().GetAll();
var categories = app.Services.GetRequiredService<IProductCategoryRepository>().GetAll();
var products = app.Services.GetRequiredService<IProductRepository>().GetAll();
var tabs = app.Services.GetRequiredService<ICustomerTabRepository>().GetAll();
var orders = app.Services.GetRequiredService<IOrderRepository>().GetAll();
var orderItems = app.Services.GetRequiredService<IOrderItemRepository>().GetAll();

foreach (var role in roles)
{
    role.Users.Clear();
}

foreach (var category in categories)
{
    category.Products.Clear();
}

foreach (var user in users)
{
    user.Tabs.Clear();
    user.Role = roles.SingleOrDefault(r => r.Id == user.RoleId);
    user.Role?.Users.Add(user);
}

foreach (var product in products)
{
    product.OrderItems.Clear();
    product.Category = categories.SingleOrDefault(c => c.Id == product.ProductCategoryId);
    product.Category?.Products.Add(product);
}

foreach (var tab in tabs)
{
    tab.Orders.Clear();
    tab.OpenedByUser = users.SingleOrDefault(u => u.Id == tab.OpenedByUserId);
    tab.OpenedByUser?.Tabs.Add(tab);
}

foreach (var order in orders)
{
    order.Items.Clear();
    order.CustomerTab = tabs.SingleOrDefault(t => t.Id == order.CustomerTabId);
    order.CustomerTab?.Orders.Add(order);
}

foreach (var item in orderItems)
{
    item.Order = orders.SingleOrDefault(o => o.Id == item.OrderId);
    item.Order?.Items.Add(item);

    item.Product = products.SingleOrDefault(p => p.Id == item.ProductId);
    item.Product?.OrderItems.Add(item);
}

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
