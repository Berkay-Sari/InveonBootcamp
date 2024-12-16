using LibraryManagement.Web.Models.DTOs;
using LibraryManagement.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LibraryManagement.Web.Services;

public interface IAccountService
{
    Task<bool> LoginAsync(LoginRequest model);
    Task<IdentityResult> SignUpAsync(CreateUserRequest model);
    Task LogoutAsync();
    Task<AppUser?> GetCurrentUserAsync(ClaimsPrincipal user);
    Task<IList<string>> GetUserRolesAsync(AppUser user);
}
