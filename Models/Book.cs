using System.ComponentModel.DataAnnotations.Schema;

namespace BookHive.Models;

public class Book
{
    public int Id { get; set; } // Unique identifier for the book
    public required string Title { get; set; } // Book title
    public required string Description { get; set; } // Book title
    public required string Author { get; set; } // Author's name
    public required string Genre { get; set; } // Genre of the book
    public required string ISBN { get; set; } = Random.Shared.NextInt64(1000000000000, 9999999999999).ToString(); // International Standard Book Number
    public string PosterUrl { get; set; }
    public required int Year { get; set; } = DateTime.Now.Year; // Year of publication
    public bool Availability { get; set; } = true; // True if the book is available for borrowing
    public int? BorrowedById { get; set; } // Foreign key
    
    [ForeignKey(nameof(BorrowedById))]
    public User? BorrowedBy { get; set; } // User who borrowed the book, if any
}