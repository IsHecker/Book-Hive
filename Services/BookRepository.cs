using BookHive.Models;

namespace BookHive.Services;

public class BookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Book> GetAll()
    {
        return _context.Books;
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            throw new KeyNotFoundException();
        return book;
    }

    public async Task AddAsync(Book entity)
    {
        await _context.Books.AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Book entity)
    {
        _context.Books.Update(entity);
        await SaveChangesAsync();
    }

    public async Task<Book?> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return null;

        _context.Books.Remove(book);
        await SaveChangesAsync();
        return book;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}