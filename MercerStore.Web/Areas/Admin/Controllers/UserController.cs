using MediatR;
using MercerStore.Web.Application.Handlers.Users.Commands;
using MercerStore.Web.Application.Handlers.Users.Queries;
using MercerStore.Web.Areas.Admin.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult UserPage()
    {
        return View();
    }

    public IActionResult UsersList()
    {
        return View();
    }

    [HttpGet("[area]/[controller]/update/{userId}")]
    public async Task<IActionResult> UpdateUserProfile(string userId)
    {
        var updateUserProfileViewModel = await _mediator.Send(new GetUpdateUserProfileViewModelQuery(userId));
        return View(updateUserProfileViewModel);
    }

    [HttpPost("[area]/[controller]/update/{userId}")]
    public async Task<IActionResult> UpdateUserProfileAsync(UpdateUserProfileViewModel updateUserProfileViewModel)
    {
        if (!ModelState.IsValid) return View(updateUserProfileViewModel);
        await _mediator.Send(new AdminUpdateUserProfileCommand(updateUserProfileViewModel));

        return RedirectToAction("UpdateUserProfile");
    }
}