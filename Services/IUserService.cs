using LibraryManagement.Web.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Web.Services;

public interface IUserService
{
    Task<List<UserListItemResponse>> GetAllAsync();
    Task<ServiceResult<UpdateUserRequest>> GetByIdAsync(Guid id);
    Task<IdentityResult> CreateAsync(CreateUserRequest userDto);
    Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserRequest updateUserRequest);
    Task<IdentityResult> DeleteUserAsync(Guid id);
}