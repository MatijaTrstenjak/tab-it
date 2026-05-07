# Tab-It - Sitemap & Routing Model

| URL | Controller | Action | View/Function |
|---|---|---|---|
| `/` <br>*(fallback route)* | `HomeController` | `Index()` | `Views/Home/Index.cshtml` |
| `/pocetna` | `HomeController` | `Index()` | `Views/Home/Index.cshtml` |
| `/Home/Dashboard` | `HomeController` | `Dashboard()` | `Views/Home/Dashboard.cshtml` |
| `/Home/POS` | `HomeController` | `POS()` | `Views/Home/POS.cshtml` |
| `/proizvodi/katalog`<br>`/Products`<br>`/Products/Index` | `ProductsController` | `Index()` | `Views/Products/Index.cshtml` |
| `/Products/Details/{id}` | `ProductsController` | `Details(int id)` | `Views/Products/Details.cshtml` |
| `/CustomerTabs` | `CustomerTabsController` | `Index()` | `Views/CustomerTabs/Index.cshtml` |
| `/racuni/detalji/{id}`<br>`/CustomerTabs/Details/{id}` | `CustomerTabsController` | `Details(int id)` | `Views/CustomerTabs/Details.cshtml` |
| `/admin/korisnici`<br>`/Users`<br>`/Users/Index` | `UsersController` | `Index()` | `Views/Users/Index.cshtml` |
| `/Users/Details/{id}` | `UsersController` | `Details(int id)` | `Views/Users/Details.cshtml` |
| `/Roles` | `RolesController` | `Index()` | `Views/Roles/Index.cshtml` |
| `/Roles/Details/{id}` | `RolesController` | `Details(int id)` | `Views/Roles/Details.cshtml` |
| `/ProductCategories` | `ProductCategoriesController`| `Index()` | `Views/ProductCategories/Index.cshtml` |
| `/ProductCategories/Details/{id}` | `ProductCategoriesController`| `Details(int id)`| `Views/ProductCategories/Details.cshtml` |
| `/Orders` | `OrdersController` | `Index()` | `Views/Orders/Index.cshtml` |
| `/Orders/Details/{id}` | `OrdersController` | `Details(int id)` | `Views/Orders/Details.cshtml` |
| `/OrderItems` | `OrderItemsController` | `Index()` | `Views/OrderItems/Index.cshtml` |
| `/OrderItems/Details/{id}`| `OrderItemsController` | `Details(int id)`| `Views/OrderItems/Details.cshtml` |

*Note: Classic routes without explicit `[Route]` metadata fall back to the global pattern `{controller=Home}/{action=Index}/{id?}` configured in Program.cs.*
