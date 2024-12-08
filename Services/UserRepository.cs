using BookHive.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Services;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }

    public User? FindUser(string username, string password)
    {
        return _context.Users.FirstOrDefault(user => user.Name == username && user.Password == password);
    }

    public async Task<List<Book>?> GetBorrowedBooks(int userId)
    {
        return (await GetByIdAsync(userId)).BorrowedBooks;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _context.Users.Include(u => u.BorrowedBooks).FirstAsync(u => u.Id == id);
        if (user == null)
            throw new KeyNotFoundException();
        return user;
    }

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task<bool> UpdateProfileAsync(User updatedUser)
    {
        var user = await GetByIdAsync(updatedUser.Id);
        var isEmailUpdated = user.Email != updatedUser.Email;
        if (user != null)
        {
            if (isEmailUpdated &&
                await _context.Users.AnyAsync(u => u.Email == updatedUser.Email)) // Another account with the same email
            {
                return false;
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
        }

        await SaveChangesAsync();
        return true;
    }

    public async Task AddBorrowedBooks(int userId, Book book)
    {
        var user = await GetByIdAsync(userId);
        user.BorrowedBooks ??= [];
        user.BorrowedBooks.Add(book);
        book.BorrowedById = userId;
        book.BorrowedBy = user;
        await SaveChangesAsync();
    }

    public async Task<Book> RemoveBorrowedBooks(int userId, int bookId)
    {
        var user = await GetByIdAsync(userId);
        var book = user.BorrowedBooks!.Find(book => book.Id == bookId);
        user.BorrowedBooks.Remove(book!);
        book!.BorrowedById = null;
        book.BorrowedBy = null;
        await SaveChangesAsync();
        return book;
    }

    public async Task DeleteAsync(int id)
    {
        var user = _context.Users
            .Include(u => u.BorrowedBooks) // Load borrow
            .FirstOrDefault(u => u.Id == id);

        if (user == null)
            return;
        
        if (user.BorrowedBooks != null)
        {
            foreach (var book in user.BorrowedBooks)
            {
                book.BorrowedById = null; // Disassociate borrowed books
            }
        }

        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}