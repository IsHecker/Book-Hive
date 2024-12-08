namespace BookHive.Models;

public class User
{
    public static User Current { get; set; }
    
    public int Id { get; init; } // Unique identifier for the user
    public required string Name { get; set; } // User's name
    public required string Email { get; set; } // Email address for login
    public string Password { get; set; } // Encrypted password
    public string Role { get; set; } = "Member"; // e.g., "Admin", "Member"
    public string Status { get; set; } = "Active"; // Active or Suspended
    public List<Book>? BorrowedBooks { get; set; } = null; // Books borrowed by the user
}