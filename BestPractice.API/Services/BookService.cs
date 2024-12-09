using BestPractice.API.Data;
using BestPractice.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BestPractice.API.Services;

public class BookService(BookContext context, IRedisCacheService redisCacheService) : IBookService
{
    private const string CacheKey = "books";
    public async Task<ServiceResult<List<BookDto>>> Read()
    {
        //cache
        var cachedBooks = await redisCacheService.GetValueAsync<List<BookDto>>(CacheKey);
        if (cachedBooks != null) return ServiceResult<List<BookDto>>.Success(cachedBooks, StatusCodes.Status200OK);

        var books = await context.Books
            .AsNoTracking()
            .Select(b => BookDtoMapper.Map(b))
            .ToListAsync();

        if (books.Count == 0) return ServiceResult<List<BookDto>>.Fail("No books found", StatusCodes.Status404NotFound);

        await redisCacheService.SetValueAsync(CacheKey, books, TimeSpan.FromMinutes(30));

        return ServiceResult<List<BookDto>>.Success(books, StatusCodes.Status200OK);
    }

    public async Task<ServiceResult<BookDto>> Read(int id)
    {
        var book = await context.Books
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => BookDtoMapper.Map(b))
            .FirstOrDefaultAsync();

        if (book == null) return ServiceResult<BookDto>.Fail("Book not found", StatusCodes.Status404NotFound);
        return ServiceResult<BookDto>.Success(book, StatusCodes.Status200OK);
    }

    public async Task<ServiceResult<BookDto>> Create(BookCreateDto bookCreateDto)
    {
        var book = new Book
        {
            Title = bookCreateDto.Title,
            Author = bookCreateDto.Author,
            Quantity = bookCreateDto.Quantity
        };
        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();

        // Partial Cache Update: Append the new book no need to iterate over all books
        var cachedBooks = await redisCacheService.GetValueAsync<List<BookDto>>(CacheKey);
        if (cachedBooks != null)
        {
            var newBookDto = BookDtoMapper.Map(book);
            cachedBooks.Add(newBookDto);
            await redisCacheService.SetValueAsync(CacheKey, cachedBooks, TimeSpan.FromMinutes(30));
        }

        return ServiceResult<BookDto>.Success(BookDtoMapper.Map(book), StatusCodes.Status201Created);
    }

    public async Task<ServiceResult<BookDto>> Update(int id, BookUpdateDto bookUpdateDto)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null) return ServiceResult<BookDto>.Fail("Book not found", StatusCodes.Status404NotFound);
        context.Entry(book).State = EntityState.Modified;
        try
        {
            book.Title = bookUpdateDto.Title;
            book.Author = bookUpdateDto.Author;
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await BookAvailable(id))
                return ServiceResult<BookDto>.Fail("Book not found", StatusCodes.Status404NotFound);
            throw;

        }

        // Invalidate Cache
        await redisCacheService.Clear(CacheKey);

        //burada cache'i güncellemek yerine key'i silmekteki motivasyonum
        //tüm kitap listesi arasında update edileni bulup güncellemek vs. gibi işlemlerin request time'ı artırma potansiyeli olması.
        //buradan farklı olarak kitap eklemede sadece "append" ettiğim için orada cache'i güncelleme yoluna gittim.

        return ServiceResult<BookDto>.Success(BookDtoMapper.Map(book), StatusCodes.Status204NoContent);
    }

    public async Task<ServiceResult<BookDto>> Delete(int id)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null) return ServiceResult<BookDto>.Fail("Book not found", StatusCodes.Status404NotFound);
        context.Books.Remove(book);
        await context.SaveChangesAsync();

        // Invalidate Cache
        await redisCacheService.Clear(CacheKey); //yine aynı sebep silinecek kitabı bulmak O(n), (hash, index vs ayarlanmadığı sürece).
        return ServiceResult<BookDto>.Success(BookDtoMapper.Map(book), StatusCodes.Status200OK);
    }

    public async Task<ServiceResult<BookDto>> UpdateQuantity(int id, int quantity)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null) return ServiceResult<BookDto>.Fail("Book not found", StatusCodes.Status404NotFound);
        book.Quantity = quantity;
        await context.SaveChangesAsync();
        await redisCacheService.Clear(CacheKey);
        return ServiceResult<BookDto>.Success(BookDtoMapper.Map(book), StatusCodes.Status200OK);
    }

    public async Task<bool> BookAvailable(int id) //concurrent işlem yapılmaması için kullanılabilir.
    {
        return await context.Books.AnyAsync(x => x.Id == id);
    }

    public async Task<ServiceResult<PaginatedList<BookDto>>> Read(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            return ServiceResult<PaginatedList<BookDto>>.Fail("Page number and page size must be greater than 0",
                StatusCodes.Status400BadRequest);

        var totalBooks = await context.Books.CountAsync();
        if (totalBooks == 0)
            return ServiceResult<PaginatedList<BookDto>>.Fail("No books found", StatusCodes.Status404NotFound);

        var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

        var books = await context.Books
            .AsNoTracking()
            .Select(b => BookDtoMapper.Map(b))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var paginatedList = new PaginatedList<BookDto>(books, pageNumber, totalPages);

        return ServiceResult<PaginatedList<BookDto>>.Success(paginatedList, StatusCodes.Status200OK);
    }
}