using BookHive.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookHive.Controllers;

public class CatalogController : Controller
{
    private readonly BookRepository _repository;

    public CatalogController(BookRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        return View(_repository.GetAll());
    }

    public async Task<IActionResult> BookDetails(int id)
    {
        return View(await _repository.GetByIdAsync(id));
    }

    public IActionResult Search(string searchValue)
    {
        var books = _repository.GetAll();
        return PartialView("BookList",
            searchValue.IsNullOrEmpty() ? books : books.Where(book => book.Title.Contains(searchValue, StringComparison.OrdinalIgnoreCase)));
    }
}