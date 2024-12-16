using System.Security.Claims;
using LibraryManagement.Web.Models.DTOs;
using LibraryManagement.Web.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Web.Services;

public class UserService(UserManager<AppUser> userManager) : IUserService
{
    public Task<List<UserListItemResponse>> GetAllAsync()
    {
        var userListDto = userManager.Users
            .ProjectToType<UserListItemResponse>()
            .ToListAsync();

        return userListDto;
    }

    public async Task<ServiceResult<UpdateUserRequest>> GetByIdAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return ServiceResult<UpdateUserRequest>.Fail("User not found", StatusCodes.Status404NotFound);

        var userDto = user.Adapt<UpdateUserRequest>();
        return ServiceResult<UpdateUserRequest>.Success(userDto);
    }

    public async Task<IdentityResult> CreateAsync(CreateUserRequest userDto)
    {
        var user = userDto.Adapt<AppUser>();
        var result = await userManager.CreateAsync(user, userDto.Password);
        return result;
    }

    public async Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserRequest updateUserRequest)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "User not found" });

        updateUserRequest.Adapt(user);

        var isNameChanged = user.FirstName != updateUserRequest.FirstName || user.LastName != updateUserRequest.LastName;

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return updateResult;

        if (!isNameChanged) return IdentityResult.Success;

        var claims = new List<Claim>
        {
            new("FirstName", updateUserRequest.FirstName),
            new("LastName", updateUserRequest.LastName)
        };

        var existingClaims = await userManager.GetClaimsAsync(user);
        var claimsToRemove = existingClaims
            .Where(c => c.Type is "FirstName" or "LastName")
            .ToList();

        var removeResult = await userManager.RemoveClaimsAsync(user, claimsToRemove);
        if (!removeResult.Succeeded)
            return removeResult;

        var addResult = await userManager.AddClaimsAsync(user, claims);
        if (addResult.Succeeded)
        {
            // Refresh the security stamp only if claims were successfully replaced
            await userManager.UpdateSecurityStampAsync(user);
        }
        return addResult;
    }

    public async Task<IdentityResult> DeleteUserAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "User not found" });

        var result = await userManager.DeleteAsync(user);
        return result;
    }

}
