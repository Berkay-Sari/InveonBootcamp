using LibraryManagement.Web.Models.Entities;

namespace LibraryManagement.Web.Data.Repositories;

public class BookRepository(AppDbContext context)
    : Repository<Book, int>(context), IBookRepository
{
}