using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using tab_it.DAL;
using tab_it.Models.Domain;

namespace tab_it.Tests;

public class ApiCrudIntegrationTests
{
    public static IEnumerable<object[]> CrudEndpoints =>
        new[]
        {
            new object[] { new CrudEndpointSpec("Products", "/api/products", new[] { "Manager" }, 1, ValidProduct("API Product", "API-PROD-1"), ValidProduct("API Product Updated", "API-PROD-1-UPD"), InvalidProduct()) },
            new object[] { new CrudEndpointSpec("Product categories", "/api/product-categories", new[] { "Manager" }, 1, new { name = "API Category", description = "Created from tests" }, new { name = "API Category Updated", description = "Updated from tests" }, new { name = "", description = "Invalid" }) },
            new object[] { new CrudEndpointSpec("Inventory items", "/api/inventory-items", new[] { "Manager" }, 1, ValidInventoryItem("API Beans", "API-INV-1"), ValidInventoryItem("API Beans Updated", "API-INV-1-UPD"), InvalidInventoryItem()) },
            new object[] { new CrudEndpointSpec("Product recipe items", "/api/product-recipe-items", new[] { "Manager" }, 1, new { productId = 1, inventoryItemId = 1, quantityRequired = 0.25m }, new { productId = 2, inventoryItemId = 2, quantityRequired = 0.50m }, new { productId = 0, inventoryItemId = 0, quantityRequired = 0m }) },
            new object[] { new CrudEndpointSpec("Customer tabs", "/api/customer-tabs", new[] { "Manager" }, 1, ValidCustomerTab("API-TAB-1"), ValidCustomerTab("API-TAB-1-UPD", 8), InvalidCustomerTab()) },
            new object[] { new CrudEndpointSpec("Orders", "/api/orders", new[] { "Manager" }, 1, ValidOrder("API-ORD-1"), ValidOrder("API-ORD-1-UPD", 4.20m), InvalidOrder()) },
            new object[] { new CrudEndpointSpec("Order items", "/api/order-items", new[] { "Manager" }, 1, ValidOrderItem(2), ValidOrderItem(3), InvalidOrderItem()) },
            new object[] { new CrudEndpointSpec("Staff users", "/api/users", new[] { "Admin" }, 1, ValidStaffUser("api.user"), ValidStaffUser("api.user.updated"), InvalidStaffUser()) },
            new object[] { new CrudEndpointSpec("Staff roles", "/api/roles", new[] { "Admin" }, 1, new { name = "API Role", description = "Created from tests" }, new { name = "API Role Updated", description = "Updated from tests" }, new { name = "", description = "Invalid" }) }
        };

    [Theory]
    [MemberData(nameof(CrudEndpoints))]
    public async Task GetAll_ReturnsOk(CrudEndpointSpec spec)
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient(spec.Roles);

        var response = await client.GetAsync(spec.Route);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(JsonValueKind.Array, json.RootElement.ValueKind);
    }

    [Theory]
    [MemberData(nameof(CrudEndpoints))]
    public async Task GetById_ReturnsRecord_WhenRecordExists(CrudEndpointSpec spec)
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient(spec.Roles);

        var response = await client.GetAsync($"{spec.Route}/{spec.ExistingId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(spec.ExistingId, await ReadIdAsync(response));
    }

    [Theory]
    [MemberData(nameof(CrudEndpoints))]
    public async Task GetById_ReturnsNotFound_WhenRecordIsMissing(CrudEndpointSpec spec)
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient(spec.Roles);

        var response = await client.GetAsync($"{spec.Route}/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(CrudEndpoints))]
    public async Task Post_CreatesRecord_WhenPayloadIsValid(CrudEndpointSpec spec)
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient(spec.Roles);

        var response = await client.PostAsJsonAsync(spec.Route, spec.CreatePayload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.True(await ReadIdAsync(response) > 0);
    }

    [Theory]
    [MemberData(nameof(CrudEndpoints))]
    public async Task Post_ReturnsBadRequest_WhenPayloadIsInvalid(CrudEndpointSpec spec)
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient(spec.Roles);

        var response = await client.PostAsJsonAsync(spec.Route, spec.InvalidPayload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(CrudEndpoints))]
    public async Task Put_UpdatesRecord_WhenRecordExists(CrudEndpointSpec spec)
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient(spec.Roles);

        var response = await client.PutAsJsonAsync($"{spec.Route}/{spec.ExistingId}", spec.UpdatePayload);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync($"{spec.Route}/{spec.ExistingId}")).StatusCode);
    }

    [Theory]
    [MemberData(nameof(CrudEndpoints))]
    public async Task Put_ReturnsNotFound_WhenRecordIsMissing(CrudEndpointSpec spec)
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient(spec.Roles);

        var response = await client.PutAsJsonAsync($"{spec.Route}/999999", spec.UpdatePayload);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(CrudEndpoints))]
    public async Task Delete_RemovesRecord_WhenRecordExists(CrudEndpointSpec spec)
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient(spec.Roles);

        var response = await client.DeleteAsync($"{spec.Route}/{spec.ExistingId}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"{spec.Route}/{spec.ExistingId}")).StatusCode);
    }

    [Theory]
    [MemberData(nameof(CrudEndpoints))]
    public async Task Delete_ReturnsNotFound_WhenRecordIsMissing(CrudEndpointSpec spec)
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient(spec.Roles);

        var response = await client.DeleteAsync($"{spec.Route}/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ProtectedEndpoint_ReturnsUnauthorized_WhenUserIsAnonymous()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateUnauthenticatedClient();

        var response = await client.GetAsync("/api/products");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AdminEndpoint_ReturnsForbidden_WhenUserDoesNotHaveRequiredRole()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.GetAsync("/api/users");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ProductImages_GetAll_ReturnsOk()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.GetAsync("/api/products/1/images");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ProductImages_GetAll_ReturnsNotFound_WhenProductIsMissing()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.GetAsync("/api/products/999999/images");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ProductImages_GetById_ReturnsRecord_WhenImageExists()
    {
        using var factory = new TestWebApplicationFactory();
        var imageId = await factory.SeedProductImageAsync();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.GetAsync($"/api/products/1/images/{imageId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(imageId, await ReadIdAsync(response));
    }

    [Fact]
    public async Task ProductImages_GetById_ReturnsNotFound_WhenImageIsMissing()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.GetAsync("/api/products/1/images/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ProductImages_Post_UploadsImage_WhenPayloadIsValid()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.PostAsync("/api/products/1/images", CreateImageContent("created.png"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.True(await ReadIdAsync(response) > 0);
    }

    [Fact]
    public async Task ProductImages_Post_ReturnsBadRequest_WhenFileIsInvalid()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.PostAsync("/api/products/1/images", CreateImageContent("bad.txt", "text/plain"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ProductImages_Put_ReplacesImage_WhenPayloadIsValid()
    {
        using var factory = new TestWebApplicationFactory();
        var imageId = await factory.SeedProductImageAsync();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.PutAsync($"/api/products/1/images/{imageId}", CreateImageContent("updated.png"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(imageId, await ReadIdAsync(response));
    }

    [Fact]
    public async Task ProductImages_Put_ReturnsNotFound_WhenImageIsMissing()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.PutAsync("/api/products/1/images/999999", CreateImageContent("updated.png"));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ProductImages_Delete_RemovesImage_WhenImageExists()
    {
        using var factory = new TestWebApplicationFactory();
        var imageId = await factory.SeedProductImageAsync();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.DeleteAsync($"/api/products/1/images/{imageId}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/products/1/images/{imageId}")).StatusCode);
    }

    [Fact]
    public async Task ProductImages_Delete_ReturnsNotFound_WhenImageIsMissing()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateAuthenticatedClient("Manager");

        var response = await client.DeleteAsync("/api/products/1/images/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static async Task<int> ReadIdAsync(HttpResponseMessage response)
    {
        using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        return json.RootElement.GetProperty("id").GetInt32();
    }

    private static MultipartFormDataContent CreateImageContent(string fileName, string contentType = "image/png")
    {
        var content = new MultipartFormDataContent();
        var bytes = new ByteArrayContent(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });
        bytes.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        content.Add(bytes, "file", fileName);
        return content;
    }

    private static object ValidProduct(string name, string sku) => new
    {
        name,
        sku,
        unitPrice = 4.50m,
        isAlcoholic = false,
        availableQuantity = 12,
        lastRestockedAt = DateTime.UtcNow,
        productCategoryId = 1
    };

    private static object InvalidProduct() => new
    {
        name = "",
        sku = "",
        unitPrice = -1m,
        availableQuantity = -1,
        productCategoryId = 0
    };

    private static object ValidInventoryItem(string name, string sku) => new
    {
        name,
        sku,
        unit = InventoryUnit.Quantity,
        quantityOnHand = 10m,
        reorderLevel = 2m,
        lastRestockedAt = DateTime.UtcNow
    };

    private static object InvalidInventoryItem() => new
    {
        name = "",
        sku = "",
        quantityOnHand = -1m,
        reorderLevel = -1m
    };

    private static object ValidCustomerTab(string tabCode, int tableNumber = 7) => new
    {
        tabCode,
        tableNumber,
        openedAt = DateTime.UtcNow,
        status = TabStatus.Open,
        notes = "Created from API tests",
        paymentMethod = "Card",
        openedByUserId = 1
    };

    private static object InvalidCustomerTab() => new
    {
        tabCode = "",
        tableNumber = 0,
        openedByUserId = 0
    };

    private static object ValidOrder(string orderNumber, decimal total = 3.50m) => new
    {
        orderNumber,
        orderedAt = DateTime.UtcNow,
        status = OrderStatus.Draft,
        subtotal = total,
        discountPercent = 0m,
        total,
        customerTabId = 1
    };

    private static object InvalidOrder() => new
    {
        orderNumber = "",
        subtotal = -1m,
        discountPercent = 101m,
        total = -1m,
        customerTabId = 0
    };

    private static object ValidOrderItem(int quantity) => new
    {
        quantity,
        unitPrice = 2.50m,
        lineTotal = quantity * 2.50m,
        itemNote = "Created from API tests",
        orderId = 1,
        productId = 1
    };

    private static object InvalidOrderItem() => new
    {
        quantity = 0,
        unitPrice = -1m,
        lineTotal = -1m,
        orderId = 0,
        productId = 0
    };

    private static object ValidStaffUser(string username) => new
    {
        firstName = "API",
        lastName = "User",
        username,
        email = $"{username}@tab-it.local",
        passwordHash = "not-a-real-password-hash",
        createdAt = DateTime.UtcNow,
        isActive = true,
        roleId = 1
    };

    private static object InvalidStaffUser() => new
    {
        firstName = "",
        lastName = "",
        username = "",
        email = "not-an-email",
        passwordHash = "",
        roleId = 0
    };

    public sealed record CrudEndpointSpec(
        string Name,
        string Route,
        string[] Roles,
        int ExistingId,
        object CreatePayload,
        object UpdatePayload,
        object InvalidPayload)
    {
        public override string ToString() => Name;
    }
}

public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"tab-it-tests-{Guid.NewGuid():N}";
    private readonly string _webRootPath;

    public TestWebApplicationFactory()
    {
        _webRootPath = Path.Combine(Path.GetTempPath(), "tab-it-api-tests", _databaseName, "wwwroot");
        Directory.CreateDirectory(_webRootPath);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseWebRoot(_webRootPath);
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:TabItDbContext"] = "TestConnection",
                ["Authentication:Google:ClientId"] = "",
                ["Authentication:Google:ClientSecret"] = ""
            });
        });

        builder.ConfigureTestServices(services =>
        {
            foreach (var descriptor in services.Where(d => d.ServiceType == typeof(DbContextOptions<TabItDbContext>)).ToList())
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<TabItDbContext>(options => options.UseInMemoryDatabase(_databaseName));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                    options.DefaultForbidScheme = TestAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TabItDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        return host;
    }

    public HttpClient CreateAuthenticatedClient(params string[] roles)
    {
        var client = CreateUnauthenticatedClient();
        client.DefaultRequestHeaders.Add(TestAuthHandler.EnabledHeader, "true");
        client.DefaultRequestHeaders.Add(TestAuthHandler.RolesHeader, string.Join(",", roles));
        return client;
    }

    public HttpClient CreateUnauthenticatedClient() =>
        CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    public async Task<int> SeedProductImageAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TabItDbContext>();
        var image = new ProductImage
        {
            ProductId = 1,
            OriginalFileName = "seed.png",
            StoredFileName = "seed.png",
            RelativePath = "/uploads/products/1/seed.png",
            ContentType = "image/png",
            FileSize = 8,
            CreatedAt = DateTime.UtcNow
        };

        db.ProductImages.Add(image);
        await db.SaveChangesAsync();
        return image.Id;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing && Directory.Exists(_webRootPath))
        {
            Directory.Delete(Path.GetDirectoryName(_webRootPath)!, recursive: true);
        }
    }
}

public sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "Test";
    public const string EnabledHeader = "X-Test-Auth";
    public const string RolesHeader = "X-Test-Roles";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(EnabledHeader))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "integration-test-user"),
            new(ClaimTypes.Name, "Integration Test User")
        };

        if (Request.Headers.TryGetValue(RolesHeader, out var roleHeader))
        {
            claims.AddRange(roleHeader
                .ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    }
}
