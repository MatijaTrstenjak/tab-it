using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tab_it.Models.ViewModels;

namespace tab_it.Controllers;

[Authorize(Roles = "Admin")]
public class RolesController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Roles";
        return View(await GetRolesAsync());
    }

    public async Task<IActionResult> Details(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Role Details";
        return View(ToListItem(role));
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Create Role";
        return View(new IdentityRoleEditViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IdentityRoleEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Create Role";
            return View(model);
        }

        var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Index));
        }

        AddErrors(result);
        ViewData["Title"] = "Create Role";
        return View(model);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit Role";
        return View(new IdentityRoleEditViewModel { Id = role.Id, Name = role.Name ?? string.Empty });
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(string id, IdentityRoleEditViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Edit Role";
            return View("Edit", model);
        }

        role.Name = model.Name;
        var result = await _roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Index));
        }

        AddErrors(result);
        ViewData["Title"] = "Edit Role";
        return View("Edit", model);
    }

    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete Role";
        return View(ToListItem(role));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
        {
            return NotFound();
        }

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Index));
        }

        AddErrors(result);
        ViewData["Title"] = "Delete Role";
        return View("Delete", ToListItem(role));
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var roles = await GetRolesAsync(query);
        return PartialView("_Rows", roles);
    }

    private async Task<IReadOnlyList<IdentityRoleListItemViewModel>> GetRolesAsync(string? q = null)
    {
        var query = _roleManager.Roles.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(r => r.Name != null && r.Name.Contains(q));
        }

        return await query
            .OrderBy(r => r.Name)
            .Select(r => ToListItem(r))
            .ToListAsync();
    }

    private static IdentityRoleListItemViewModel ToListItem(IdentityRole role)
    {
        return new IdentityRoleListItemViewModel(role.Id, role.Name ?? string.Empty);
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
