using LibraryManagement.Web.Models.DTOs;
using LibraryManagement.Web.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LibraryManagement.Web.Services;

public class AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    : IAccountService
{
    public async Task<bool> LoginAsync(LoginRequest model)
    {
        var result = await signInManager.PasswordSignInAsync(
            model.UserName,
            model.Password,
            model.RememberMe,
            false);

        return result.Succeeded;
    }

    public async Task<IdentityResult> SignUpAsync(CreateUserRequest model)
    {
        var user = model.Adapt<AppUser>();

        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded) return result;

        var claims = new List<Claim>
        {
            new("FirstName", user.FirstName),
            new("LastName", user.LastName)
        };

        await userManager.AddClaimsAsync(user, claims);
        await signInManager.SignInAsync(user, false);

        return result;
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }

    public async Task<AppUser?> GetCurrentUserAsync(ClaimsPrincipal user)
    {
        return await userManager.GetUserAsync(user);
    }

    public async Task<IList<string>> GetUserRolesAsync(AppUser user)
    {
        return await userManager.GetRolesAsync(user);
    }
}