using BookHive.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

public class AdminController : Controller
{
    private readonly BookRepository _bookRepository;
    private readonly UserRepository _userRepository;
    private readonly CommentRepository _commentRepository;

    public AdminController(BookRepository bookRepository, UserRepository userRepository,
        CommentRepository commentRepository)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
    }

    public IActionResult Index()
    {
        return View(model: new
        {
            BooksCount = _bookRepository.GetAll().Count(),
            UsersCount = _userRepository.GetAll().Count(),
            ReviewsCount = _commentRepository.GetAllComments().Count()
        });
    }
}