using LibraryManagement.Web.Data.Repositories;
using LibraryManagement.Web.Models.DTOs;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Web.Services;

public class BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    : IBookService
{
    public async Task<ServiceResult<List<BookListItemResponse>>> GetAllListAsync()
    {
        var books = await bookRepository.GetAll().ToListAsync();

        var booksAsDto = books.Adapt<List<BookListItemResponse>>();

        return ServiceResult<List<BookListItemResponse>>.Success(booksAsDto);
    }

    public async Task<ServiceResult<BookDetailResponse>> GetByIdAsync(int id)
    {
        var book = await bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            return ServiceResult<BookDetailResponse>.Fail("Book not found.", StatusCodes.Status404NotFound);
        }

        var bookDetailDto = book.Adapt<BookDetailResponse>();
        return ServiceResult<BookDetailResponse>.Success(bookDetailDto);
    }

    /*
     * ornek unit of work ve repository kullanımı
     *
     * public async Task<ServiceResult> CreateAsync(CreateBookRequest bookDto)
    */
}