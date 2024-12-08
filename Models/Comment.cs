using System.ComponentModel.DataAnnotations.Schema;

namespace BookHive.Models;

public class Comment
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? BookId { get; set; }
    public string Content { get; set; }
    public bool IsApproved { get; set; } = false;
    public DateTime DatePosted { get; set; }
    
    [ForeignKey(nameof(BookId))]
    public Book Book { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
    

    public string GetShortComment()
    {
        return Content is { Length: > 20 } ? Content[..20] + "..." : Content;
    }
}