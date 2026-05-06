---
description: Helper skill for scaffolding Create/Edit forms in ASP.NET Core MVC.
pattern: "**/*Controller.cs"
---

# Edit/Create Form Skill

When the user asks to create an "edit form" or "create form" for a Model, follow these rules:

1. **Repository Interface:** Add `void Add(Entity entity);` (and `void Update(Entity entity);` if editing) to the relevant interface (e.g., `IEntityRepository`).
2. **EF Repository:** Implement the `Add` method inside the `EF...Repository` class using the injected DbContext (`_context.Entities.Add(entity); _context.SaveChanges();`).
3. **Controller:** Add a `GET Create()` method returning the form view. Add a `[HttpPost] [ValidateAntiForgeryToken] Create(Entity model)` method to process form submission.
4. **View:** Create the `Create.cshtml` explicitly utilizing ASP.NET Core Form Tag Helpers (`asp-action`, `asp-for`, `asp-validation-for`) and include standard Bootstrap or custom CSS classes.
5. **Dashboard / Navigation:** Ensure this newly created page is reachable via the Dashboard or the Index list.
