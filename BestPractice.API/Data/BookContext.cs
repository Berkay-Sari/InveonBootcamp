using BestPractice.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BestPractice.API.Data;

public class BookContext(DbContextOptions<BookContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
}