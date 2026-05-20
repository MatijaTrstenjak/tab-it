using Microsoft.AspNetCore.Mvc;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers;

public class UsersController : Controller
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [Route("/admin/korisnici")]
    [Route("/Users")]
    [Route("/Users/Index")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Users";
        return View(_userRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var user = _userRepository.GetById(id);
        if (user is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "User Details";
        return View(user);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Create User";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(tab_it.Models.Domain.User user)
    {
        if (ModelState.IsValid)
        {
            _userRepository.Add(user);
            return RedirectToAction(nameof(Index));
        }
        
        ViewData["Title"] = "Create User";
        return View(user);
    }

    public IActionResult Edit(int id)
    {
        var user = _userRepository.GetById(id);
        if (user is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Edit User";
        return View(user);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        var user = _userRepository.GetById(id);
        if (user is null)
        {
            return NotFound();
        }

        var updated = await TryUpdateModelAsync(user);
        if (updated && ModelState.IsValid)
        {
            _userRepository.Update(user);
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Edit User";
        return View(user);
    }

    public IActionResult Delete(int id)
    {
        var user = _userRepository.GetById(id);
        if (user is null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Delete User";
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var user = _userRepository.GetById(id);
        if (user is null)
        {
            return NotFound();
        }

        _userRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Search(string? q)
    {
        var query = (q ?? string.Empty).Trim();
        var users = _userRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(query))
        {
            users = users
                .Where(u => u.FirstName.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || u.LastName.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || u.Username.Contains(query, StringComparison.OrdinalIgnoreCase)
                            || u.Email.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return PartialView("_Rows", users);
    }
}
