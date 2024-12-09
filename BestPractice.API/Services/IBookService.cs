using BestPractice.API.Models;

namespace BestPractice.API.Services;

public interface IBookService
{
    Task<ServiceResult<List<BookDto>>> Read();
    Task<ServiceResult<BookDto>> Read(int id);
    Task<ServiceResult<BookDto>> Create(BookCreateDto bookCreateDto);
    Task<ServiceResult<BookDto>> Update(int id, BookUpdateDto bookUpdateDto);
    Task<ServiceResult<BookDto>> Delete(int id);
    Task<ServiceResult<BookDto>> UpdateQuantity(int id, int quantity);
    Task<bool> BookAvailable(int id);
    Task<ServiceResult<PaginatedList<BookDto>>> Read(int pageNumber, int pageSize);
}