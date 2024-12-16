using LibraryManagement.Web.Models.DTOs;
using LibraryManagement.Web.Models.Entities;
using LibraryManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers;

public class AccountController(IAccountService accountService) : BaseController
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var success = await accountService.LoginAsync(model);
        if (success) return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, "Invalid username and/or password.");
        return View(model);
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(CreateUserRequest model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await accountService.SignUpAsync(model);
        if (result.Succeeded) return RedirectToAction("Index", "Home");

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await accountService.LogoutAsync();
        return RedirectToAction("Login", "Account");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await accountService.GetCurrentUserAsync(User);
        if (user == null) return View("NotFound");

        var roles = await accountService.GetUserRolesAsync(user);
        ViewBag.Roles = roles;

        return View(user);
    }

    public IActionResult AccessDenied()
    {
        return View("AccessDenied");
    }
}
