using BookHive.Models;
using BookHive.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

public class ManageReviewsController : Controller
{
    private readonly CommentRepository _commentRepository;
    private readonly ActivityLogRepository _activityLogRepository;

    public ManageReviewsController(CommentRepository commentRepository, ActivityLogRepository activityLogRepository)
    {
        _commentRepository = commentRepository;
        _activityLogRepository = activityLogRepository;
    }

    public IActionResult Index()
    {
        return View(_commentRepository.GetUnapprovedComments());
    }

    public async Task<EmptyResult> ApproveReview(int id)
    {
        var comment = _commentRepository.GetCommentById(id);
        comment!.IsApproved = true;
        
        await _activityLogRepository.LogActivity(new ActivityLog
        {
            UserId = comment.User!.Id,
            Action = Activities.Commented + " on",
            BookName = comment.Book.Title,
            ActionDetails = $"\"{comment.GetShortComment()}\"",
            Timestamp = DateTime.Now,
        });

        await _commentRepository.SaveChangesAsync();
        return Empty;
    }
    
    public async Task<EmptyResult> RejectReview(int id)
    {
        await _commentRepository.DeleteComment(id);
        return Empty;
    }
}