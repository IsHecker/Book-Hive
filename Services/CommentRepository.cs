using BookHive.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Services;

public class CommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Comment> GetAllComments()
    {
        return _context.Comments
            .Include(c => c.User)
            .Include(c => c.Book);
    }

    public IEnumerable<Comment> GetUnapprovedComments()
    {
        return _context.Comments
            .Include(c => c.User)
            .Include(c => c.Book)
            .Where(c => c.IsApproved == false);
    }

    public IEnumerable<Comment> GetBookComments(int bookId)
    {
        return _context.Comments
            .Include(c => c.User)
            .Include(c => c.Book)
            .Where(c => c.BookId == bookId && c.IsApproved == true);
    }

    public Comment? GetCommentById(int id)
    {
        return _context.Comments
            .Include(c => c.User)
            .Include(c => c.Book).FirstOrDefault(c => c.Id == id);
    }

    public async Task AddCommentAsync(Comment comment)
    {
        _context.Comments.Add(comment);

        await SaveChangesAsync();
        await _context.Entry(comment)
            .Reference(c => c.Book)
            .LoadAsync();
    }

    public async Task DeleteComment(int id)
    {
        _context.Comments.Remove(GetCommentById(id)!);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}