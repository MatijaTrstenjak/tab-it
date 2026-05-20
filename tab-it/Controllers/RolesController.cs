using Microsoft.AspNetCore.Mvc;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

public class RolesController : Controller
{
    private readonly IRoleRepository _roleRepository;

    public RolesController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Roles";
        return View(_roleRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var role = _roleRepository.GetById(id);
        if (role is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Role Details";
        return View(role);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Create Role";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(tab_it.Models.Domain.Role role)
    {
        if (ModelState.IsValid)
        {
            _roleRepository.Add(role);
            return RedirectToAction(nameof(Index));
        }
        
        ViewData["Title"] = "Create Role";
        return View(role);
    }

    public IActionResult Edit(int id)
    {
        var role = _roleRepository.GetById(id);
        if (role is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit Role";
        return View(role);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var role = _roleRepository.GetById(id);
        if (role is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(role);
        if (updated && ModelState.IsValid)
        {
            _roleRepository.Update(role);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Edit Role";
        return View(role);
    }

    public IActionResult Delete(int id)
    {
        var role = _roleRepository.GetById(id);
        if (role is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete Role";
        return View(role);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var role = _roleRepository.GetById(id);
        if (role is null)
        {
            return NotFound();
        }

        _roleRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var roles = _roleRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            roles = roles
                .Where(r => r.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || r.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_Rows", roles);
    }
}
