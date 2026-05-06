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
}
