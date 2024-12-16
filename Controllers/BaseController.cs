using LibraryManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryManagement.Web.Controllers;
public class BaseController : Controller
{
    protected NavbarViewModel GetNavbarModel()
    {
        var viewModel = new NavbarViewModel
        {
            IsAuthenticated = User.Identity is { IsAuthenticated: true }
        };

        if (!viewModel.IsAuthenticated) return viewModel;
        var firstName = User.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
        var lastName = User.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value;
        viewModel.UserInitials = $"{firstName?[0]}{lastName?[0]}";

        return viewModel;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ViewResult)
            ViewBag.NavbarModel = GetNavbarModel();
        base.OnActionExecuted(context);
    }

    protected IActionResult HandleResult(IdentityResult result, object viewModel, string redirectAction = "Index")
    {
        if (result.Succeeded)
        {
            return RedirectToAction(redirectAction);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(viewModel);
    }
}

