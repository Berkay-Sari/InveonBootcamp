using LibraryManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers;

[Authorize]
public class BooksController(IBookService bookService) : BaseController
{
    public async Task<IActionResult> Index()
    {
        var books = await bookService.GetAllListAsync();

        return books.Failed ? View("NotFound") : View(books.Data);
    }

    public async Task<IActionResult> Details(int id)
    {
        var book = await bookService.GetByIdAsync(id);
        return book.Failed ? View("NotFound") : View(book.Data);
    }
}