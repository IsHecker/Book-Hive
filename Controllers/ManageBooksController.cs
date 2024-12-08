using BookHive.Models;
using BookHive.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

public class ManageBooksController : Controller
{
    private readonly BookRepository _bookRepository;

    public ManageBooksController(BookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public IActionResult Index()
    {
        return View(model: _bookRepository.GetAll());
    }

    public IActionResult AddBookView()
    {
        return View("AddBook");
    }

    public async Task<IActionResult> AddBook(Book book, IFormFile poster)
    {
        book.PosterUrl = await PosterSaver.SavePoster(poster);
        await _bookRepository.AddAsync(book);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EditBookView(int id)
    {
        return View("EditBook", await _bookRepository.GetByIdAsync(id));
    }

    public async Task<IActionResult> EditBook(Book newBook, IFormFile poster)
    {
        var book = await _bookRepository.GetByIdAsync(newBook.Id);
        book.Title = newBook.Title;
        book.Author = newBook.Author;
        book.Genre = newBook.Genre;
        book.Description = newBook.Description;
        book.PosterUrl = await PosterSaver.SavePoster(poster);
        await _bookRepository.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteBook(int id)
    {
        if (await _bookRepository.DeleteAsync(id) == null)
            return StatusCode(400, new { error = "Book ID don't exist" });

        return Empty;
    }
}