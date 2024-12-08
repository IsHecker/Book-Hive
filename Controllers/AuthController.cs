using BookHive.Models;
using BookHive.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

public class AuthController : Controller
{
    private readonly UserRepository _userRepository;

    public AuthController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult LoginProcess(string username, string password)
    {
        var user = _userRepository.FindUser(username, password);

        if (user == null)
            return View("Login", "Username or Password Incorrect!");

        if (user.Status == "Suspended")
            return View("Login", "Your Account is Suspended for a while!");

        return AcceptAuth(user.Id, username, user.Role);
    }

    public IActionResult Register()
    {
        return View();
    }

    public async Task<IActionResult> RegisterProcess(User user)
    {
        if (_userRepository.GetAll().Any(u => u.Email == user.Email)) // Another account with the same email
        {
            return View("Register", "Email already taken!");
        }

        await _userRepository.AddAsync(user);
        return AcceptAuth(user.Id, user.Name, user.Role);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }

    private LocalRedirectResult AcceptAuth(int userId, string username, string role)
    {
        var isAdmin = role == "Admin";
        var path = HttpContext.Session.GetString("LinkToVisit");
        HttpContext.Session.SetInt32("Auth", 1);
        HttpContext.Session.SetString("Username", username);
        HttpContext.Session.SetInt32("UserId", userId);

        if (!isAdmin)
            return LocalRedirectPermanent(path ?? "/");

        HttpContext.Session.SetString("IsAdmin", "1");
        return LocalRedirectPermanent("~/Admin/");
    }
}