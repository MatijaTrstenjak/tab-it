---
description: Entity Framework Core helper skill. Use when creating or updating models, repositories or generating migrations.
pattern: "**/*.cs"
---

# EF Core Configuration Skill

When the user asks to modify a database model or create a new Entity Framework table in this project, you MUST adhere to the following rules:

1. **Annotations:** Ensure the model has a `[Key]` attribute for its primary key.
2. **Foreign Keys:** Add `[ForeignKey("PropertyName")]` when defining relational IDs mapping to parent entities.
3. **Lazy Loading via Virtual:** Define navigation properties and collection navigational properties as `virtual` to allow for lazy loading.
4. **Collections Array Initialization:** Define collection properties accurately: `public virtual ICollection<T> Items { get; set; } = new List<T>();`
5. **Execution Commands:** After writing the models or DB Context changes, provide the exact commands the user needs to run to trigger the update:
   - `dotnet ef migrations add <DescriptiveName> --context TabItDbContext`
   - `dotnet ef database update --context TabItDbContext`
