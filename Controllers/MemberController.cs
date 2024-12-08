using BookHive.Models;
using BookHive.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

public class MemberController : Controller
{
    private readonly BookRepository _bookRepository;
    private readonly UserRepository _userRepository;
    private readonly ActivityLogRepository _activityLogRepository;

    public MemberController(
        BookRepository repository,
        UserRepository userRepository,
        ActivityLogRepository activityLogRepository)
    {
        _bookRepository = repository;
        _userRepository = userRepository;
        _activityLogRepository = activityLogRepository;
    }

    public async Task<IActionResult> AddBookToCollection([FromQuery] int bookId)
    {
        var userId = HttpContext.Session.GetUserId()!.Value;
        var book = await _bookRepository.GetByIdAsync(bookId);
        await _userRepository.AddBorrowedBooks(userId, book);

        await _activityLogRepository.LogActivity(new ActivityLog
        {
            UserId = userId,
            Action = Activities.Added.ToString(),
            BookName = book.Title,
            ActionDetails = "to your Collection",
            Timestamp = DateTime.Now,
        });
        
        return Empty;
    }

    public async Task<IActionResult> RemoveBookFromCollection([FromQuery] int bookId)
    {
        var shit = HttpContext.Request;
        var userId = HttpContext.Session.GetUserId()!.Value;
        var book = await _userRepository.RemoveBorrowedBooks(userId, bookId);

        await _activityLogRepository.LogActivity(new ActivityLog
        {
            UserId = userId,
            Action = Activities.Returned.ToString(),
            BookName = book.Title,
            ActionDetails = null,
            Timestamp = DateTime.Now,
        });

        return PartialView("BookCollection", await _userRepository.GetBorrowedBooks(userId));
    }

    public IActionResult UpdateActivityLog()
    {
        var userId = HttpContext.Session.GetUserId()!.Value;
        return PartialView("Activity", _activityLogRepository.GetUserActivityLogs(userId));
    }

    public async Task<IActionResult> EditProfile(string username, string email)
    {
        var userId = HttpContext.Session.GetUserId()!.Value;
        var isUpdated = await _userRepository.UpdateProfileAsync(new User
        {
            Id = userId,
            Email = email,
            Name = username,
        });

        if (isUpdated) return new EmptyResult();

        Response.StatusCode = StatusCodes.Status400BadRequest;
        return Json(new { error = "Invalid input", details = "Email already taken!" });
    }


    public async Task<IActionResult> Dashboard()
    {
        var userId = HttpContext.Session.GetUserId()!.Value;
        return View(new
        {
            ActivityLogs = _activityLogRepository.GetUserActivityLogs(userId),
            BookCollection = await _userRepository.GetBorrowedBooks(userId),
            User = await _userRepository.GetByIdAsync(userId)
        });
    }
}