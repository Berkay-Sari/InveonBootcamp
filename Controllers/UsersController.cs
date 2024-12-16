using LibraryManagement.Web.Models.DTOs;
using LibraryManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController(IUserService userService) : BaseController
{
    public async Task<IActionResult> Index()
    {
        var users = await userService.GetAllAsync();
        return View(users);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserRequest userDto)
    {
        var result = await userService.CreateAsync(userDto);
        return HandleResult(result, userDto);
    }

    public async Task<IActionResult> Update(Guid id)
    {
        var result = await userService.GetByIdAsync(id);
        return result.Succeeded ? View(result.Data) : View("NotFound");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest updateUserRequest)
    {
        if (!ModelState.IsValid)
            return View(updateUserRequest);

        var result = await userService.UpdateUserAsync(id, updateUserRequest);
        return HandleResult(result, updateUserRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await userService.DeleteUserAsync(id);
        return result.Succeeded ? RedirectToAction(nameof(Index)) : View("NotFound");
    }
}