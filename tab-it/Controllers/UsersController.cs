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
}
