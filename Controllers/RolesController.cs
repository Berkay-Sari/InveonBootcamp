using LibraryManagement.Web.Models.DTOs;
using LibraryManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers;

[Authorize(Roles = "Admin")]
public class RolesController(IRoleService roleService)
    : BaseController
{
    public async Task<IActionResult> Index()
    {
        var roles = await roleService.GetAllAsync();
        
        return View(roles);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleRequest roleDto)
    {
        if (!ModelState.IsValid) return View(roleDto);
        var result = await roleService.CreateAsync(roleDto);
        return HandleResult(result, roleDto);
    }

    public async Task<IActionResult> Update(Guid id)
    {
        var role = await roleService.GetByIdAsync(id);
        return role.Succeeded ? View(role.Data) : View("NotFound");
    }

    [HttpPost]
    public async Task<IActionResult> Update(Guid id, UpdateRoleRequest updateRoleRequest)
    {
        if (!ModelState.IsValid) return View(updateRoleRequest);
        var result = await roleService.UpdateAsync(id, updateRoleRequest);
        return HandleResult(result, updateRoleRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await roleService.DeleteAsync(id);
        return result.Succeeded ? RedirectToAction(nameof(Index)) : View("NotFound");
    }
    public async Task<IActionResult> ManageUserRoles(Guid userId)
    {
        var userRoles = await roleService.GetUserRolesByIdAsync(userId);
        ViewBag.UserId = userId;
        return userRoles.Succeeded ? View(userRoles.Data) : View("NotFound");
    }

    [HttpPost]
    public async Task<IActionResult> ManageUserRoles(Guid userId, List<UserRoleViewModel> model)
    {
        var result = await roleService.UpdateUserRolesAsync(userId, model);
        return HandleResult(result, model);
    }
}
