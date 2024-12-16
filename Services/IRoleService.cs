using LibraryManagement.Web.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Web.Services;
public interface IRoleService
{
    Task<List<RoleListItemResponse>> GetAllAsync();

    Task<ServiceResult<UpdateRoleRequest>> GetByIdAsync(Guid id);
    Task<IdentityResult> CreateAsync(CreateRoleRequest createRoleDto);
    Task<IdentityResult> UpdateAsync(Guid id, UpdateRoleRequest updateRoleDto);
    Task<IdentityResult> DeleteAsync(Guid id);
    Task<ServiceResult<List<UserRoleViewModel>>> GetUserRolesByIdAsync(Guid userId);
    Task<IdentityResult> UpdateUserRolesAsync(Guid userId, List<UserRoleViewModel> model);
}

