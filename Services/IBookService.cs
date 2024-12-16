using LibraryManagement.Web.Models.DTOs;
namespace LibraryManagement.Web.Services;
public interface IBookService
{
    Task<ServiceResult<List<BookListItemResponse>>> GetAllListAsync();
    Task<ServiceResult<BookDetailResponse>> GetByIdAsync(int id);

}