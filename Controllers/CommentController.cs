using BookHive.Models;
using BookHive.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

public class CommentController : Controller
{
    private readonly UserRepository _userRepository;
    private readonly CommentRepository _commentRepository;

    public CommentController(UserRepository repository, CommentRepository commentRepository)
    {
        this._userRepository = repository;
        _commentRepository = commentRepository;
    }

    public async Task<IActionResult> PostReview(int bookId, string comment)
    {
        var user = HttpContext.Session.GetString("Username");
        var userId = HttpContext.Session.GetUserId();
        var newComment = new Comment
        {
            BookId = bookId,
            Content = comment,
            UserId = userId,
            DatePosted = DateTime.Now
        };
        await _commentRepository.AddCommentAsync(newComment);
        
        return PartialView("Comments", _commentRepository.GetBookComments(bookId));
    }

    public IActionResult EditReview(string comment)
    {
        return Ok();
    }

    public IActionResult DeleteReview(int userId)
    {
        return Ok();
    }

    public IActionResult GetReviews(int bookId)
    {
        return Ok();
    }
}