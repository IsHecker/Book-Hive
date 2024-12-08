using BookHive.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Services;

public class SeedData
{
    public static void SeedDatabase(AppDbContext context)
    {
        context.Database.Migrate();
        if (context.Books.Any()
            || context.Users.Any()
            || context.ActivityLogs.Any()) return;

        var users = new List<User>
        {
            new User
            {
                Name = "Alice Johnson",
                Email = "alice.johnson@example.com",
                Password = "p",
                Role = "Member",
                Status = "Active",
                BorrowedBooks = [] // This will be populated later
            },
            new User
            {
                Name = "Bob Smith",
                Email = "bob.smith@example.com",
                Password = "p",
                Role = "Member",
                Status = "Active",
                BorrowedBooks = [] // This will be populated later
            },
            new User
            {
                Name = "Charlie Brown",
                Email = "charlie.brown@example.com",
                Password = "p",
                Role = "Admin",
                Status = "Active",
                BorrowedBooks = null // No borrowed books for the admin
            }
        };

        var books = new List<Book>
        {
            new Book
            {
                Title = "The Great Gatsby",
                Description = "The Great Gatsby is the best of my life.",
                Author = "F. Scott Fitzgerald",
                Genre = "Classic Fiction",
                ISBN = "9780743273565",
                Year = 1925,
                Availability = true,
                BorrowedBy = null // Not borrowed yet
            },
            new Book
            {
                Title = "To Kill a Mockingbird",
                Description = "To Kill a Mockingbird is the best of my life.",
                Author = "Harper Lee",
                Genre = "Classic Fiction",
                ISBN = "9780061120084",
                Year = 1960,
                Availability = false,
                BorrowedById = 1, // Borrowed by Alice
                BorrowedBy = users[0] // Reference to Alice
            },
            new Book
            {
                Title = "1984",
                Description = "1984 will be the best of my life.",
                Author = "George Orwell",
                Genre = "Dystopian",
                ISBN = "9780451524935",
                Year = 1949,
                Availability = false,
                BorrowedById = 2, // Borrowed by Bob
                BorrowedBy = users[1] // Reference to Bob
            },
            new Book
            {
                Title = "Pride and Prejudice",
                Description = "Pride and Prejudice is the best of my life.",
                Author = "Jane Austen",
                Genre = "Romance",
                ISBN = "9780141040349",
                Year = 1813,
                Availability = true,
                BorrowedBy = null // Not borrowed yet
            }
        };

        var activityLogs = new List<ActivityLog>
        {
            new ActivityLog
            {
                UserId = 1,
                Action = "Borrowed",
                BookName = "To Kill a Mockingbird",
                ActionDetails = null,
                Timestamp = DateTime.Now.AddDays(-3)
            },
            new ActivityLog
            {
                UserId = 2,
                Action = "Borrowed",
                BookName = "1984",
                ActionDetails = null,
                Timestamp = DateTime.Now.AddDays(-1)
            },
            new ActivityLog
            {
                UserId = 3,
                Action = "Added",
                BookName = "Pride and Prejudice",
                ActionDetails = "to the library",
                Timestamp = DateTime.Now
            }
        };

        users[0].BorrowedBooks!.Add(books[1]); // Alice -> To Kill a Mockingbird
        users[1].BorrowedBooks!.Add(books[2]); // Bob -> 1984


        context.Users.AddRange(users);
        context.Books.AddRange(books);
        context.ActivityLogs.AddRange(activityLogs);
        context.SaveChanges();
    }
}