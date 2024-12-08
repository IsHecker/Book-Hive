using BookHive.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

public class ManageUsersController : Controller
{
    private readonly UserRepository _userRepository;

    public ManageUsersController(UserRepository bookRepository)
    {
        _userRepository = bookRepository;
    }

    public async Task<IActionResult> Index()
    {
        return View(model: _userRepository.GetAll());
    }

    public EmptyResult ModifyAccountState(int id)
    {
        var user = _userRepository.GetByIdAsync(id).Result;
        user.Status = user.Status == "Active" ? "Suspended" : "Active";
        _userRepository.SaveChangesAsync().Wait();
        return Empty;
    }

    public async Task<EmptyResult> DeleteAccount(int id)
    {
        await _userRepository.DeleteAsync(id);
        return Empty;
    }
}