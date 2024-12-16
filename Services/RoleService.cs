using LibraryManagement.Web.Models.DTOs;
using LibraryManagement.Web.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Web.Services;

public class RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager) : IRoleService
{
    public async Task<List<RoleListItemResponse>> GetAllAsync()
    {
        var roleListDto = await roleManager.Roles
            .ProjectToType<RoleListItemResponse>()
            .ToListAsync();
        return roleListDto;
    }

    public async Task<ServiceResult<UpdateRoleRequest>> GetByIdAsync(Guid id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());
        if (role == null)
            return ServiceResult<UpdateRoleRequest>.Fail("Role not found", StatusCodes.Status404NotFound);

        var roleDto = role.Adapt<UpdateRoleRequest>();
        return ServiceResult<UpdateRoleRequest>.Success(roleDto);
    }

    public async Task<IdentityResult> CreateAsync(CreateRoleRequest roleDto)
    {
        var role = roleDto.Adapt<AppRole>();
        var result = await roleManager.CreateAsync(role);
        return result;
    }

    public async Task<IdentityResult> UpdateAsync(Guid id, UpdateRoleRequest roleDto)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());
        if (role == null)
            return IdentityResult.Failed(new IdentityError { Code = "RoleNotFound", Description = "Role not found" });
        roleDto.Adapt(role);
        var result = await roleManager.UpdateAsync(role);
        return result;
    }

    public async Task<IdentityResult> DeleteAsync(Guid id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());
        if (role == null)
            return IdentityResult.Failed(new IdentityError { Code = "RoleNotFound", Description = "Role not found" });
        var result = await roleManager.DeleteAsync(role);
        return result;
    }

    public async Task<ServiceResult<List<UserRoleViewModel>>> GetUserRolesByIdAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return ServiceResult<List<UserRoleViewModel>>.Fail("User not found", StatusCodes.Status404NotFound);

        var roles = await roleManager.Roles.ToListAsync();
        var userRoles = await userManager.GetRolesAsync(user);
        var model = roles.Select(r => new UserRoleViewModel(
            r.Id,
            r.Name!, 
            userRoles.Contains(r.Name!)
        )).ToList();
        return ServiceResult<List<UserRoleViewModel>>.Success(model);
    }


    public async Task<IdentityResult> UpdateUserRolesAsync(Guid userId, List<UserRoleViewModel> model)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "User not found" });

        var userRoles = await userManager.GetRolesAsync(user);

        var rolesToAdd = model
            .Where(r => r.IsAssigned && !userRoles.Contains(r.RoleName))
            .Select(r => r.RoleName)
            .ToList();

        var rolesToRemove = model
            .Where(r => !r.IsAssigned && userRoles.Contains(r.RoleName))
            .Select(r => r.RoleName)
            .ToList();

        if (rolesToAdd.Count > 0)
        {
            var addResult = await userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
                return IdentityResult.Failed(addResult.Errors.ToArray());
        }

        if (rolesToRemove.Count > 0)
        {
            var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
                return IdentityResult.Failed(removeResult.Errors.ToArray());
        }

        await userManager.UpdateSecurityStampAsync(user); // Refresh the security stamp to force re-authentication
        return IdentityResult.Success;
    }


}