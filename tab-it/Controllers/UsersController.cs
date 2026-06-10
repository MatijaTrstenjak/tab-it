using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tab_it.Models.Domain;
using tab_it.Models.ViewModels;

namespace tab_it.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [Route("/admin/korisnici")]
    [Route("/Users")]
    [Route("/Users/Index")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Users";
        return View(await GetIdentityUsersAsync());
    }

    public async Task<IActionResult> Details(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "User Details";
        return View(await ToListItemAsync(user));
    }

    public IActionResult Create()
    {
        return RedirectToAction("Register", "Account", new { returnUrl = Url.Action(nameof(Index), "Users") });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(tab_it.Models.Domain.User user)
    {
        return RedirectToAction(nameof(Create));
    }

    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit User";
        return View(await ToEditModelAsync(user));
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(string id, IdentityUserEditViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.AvailableRoles = await GetAvailableRolesAsync();
            return View("Edit", model);
        }

        user.Email = model.Email;
        user.UserName = model.UserName;
        user.OIB = model.OIB;
        user.JMBG = model.JMBG;
        user.EmailConfirmed = model.EmailConfirmed;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            AddErrors(updateResult);
            model.AvailableRoles = await GetAvailableRolesAsync();
            return View("Edit", model);
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var selectedRoles = model.SelectedRoles.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var rolesToRemove = currentRoles.Except(selectedRoles, StringComparer.OrdinalIgnoreCase).ToList();
        var rolesToAdd = selectedRoles.Except(currentRoles, StringComparer.OrdinalIgnoreCase).ToList();

        if (rolesToRemove.Count > 0)
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                AddErrors(removeResult);
                model.AvailableRoles = await GetAvailableRolesAsync();
                return View("Edit", model);
            }
        }

        if (rolesToAdd.Count > 0)
        {
            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                AddErrors(addResult);
                model.AvailableRoles = await GetAvailableRolesAsync();
                return View("Edit", model);
            }
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete User";
        return View(await ToListItemAsync(user));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var users = await GetIdentityUsersAsync(query);

        return PartialView("_Rows", users);
    }

    private async Task<IReadOnlyList<IdentityUserListItemViewModel>> GetIdentityUsersAsync(string? q = null)
    {
        var query = _userManager.Users.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(u =>
                (u.Email != null && u.Email.Contains(q)) ||
                (u.UserName != null && u.UserName.Contains(q)) ||
                u.OIB.Contains(q) ||
                u.JMBG.Contains(q));
        }

        var users = await query.OrderBy(u => u.Email).ToListAsync();
        var result = new List<IdentityUserListItemViewModel>();
        foreach (var user in users)
        {
            result.Add(await ToListItemAsync(user));
        }

        return result;
    }

    private async Task<IdentityUserListItemViewModel> ToListItemAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return new IdentityUserListItemViewModel(
            user.Id,
            user.Email ?? string.Empty,
            user.UserName ?? string.Empty,
            user.OIB,
            user.JMBG,
            user.EmailConfirmed,
            roles.ToList());
    }

    private async Task<IdentityUserEditViewModel> ToEditModelAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return new IdentityUserEditViewModel
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            OIB = user.OIB,
            JMBG = user.JMBG,
            EmailConfirmed = user.EmailConfirmed,
            SelectedRoles = roles.ToList(),
            AvailableRoles = await GetAvailableRolesAsync()
        };
    }

    private async Task<List<string>> GetAvailableRolesAsync()
    {
        return await _roleManager.Roles
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .Select(r => r.Name!)
            .ToListAsync();
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
