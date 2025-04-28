using MediatR;
using MercerStore.Web.Application.Handlers.Users.Commands;
using MercerStore.Web.Application.Handlers.Users.Queries;
using MercerStore.Web.Application.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc;

[Authorize(Policy = "BlacklistRolesPolicy")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> UserProfile()
    {
        var userProfileViewModel = await _mediator.Send(new GetUserProfileQuery());
        return View(userProfileViewModel);
    }

    public async Task<IActionResult> EditUserProfile()
    {
        var userProfileViewModel = await _mediator.Send(new GetUserProfileForEditQuery());
        return View(userProfileViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditUserProfile(UserProfileViewModel userProfileViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Ошибка редактирования профиля");
            return View(userProfileViewModel);
        }

        var result = await _mediator.Send(new UpdateUserProfileCommand(userProfileViewModel));
        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.ErrorMessage);
            return View(userProfileViewModel);
        }

        return RedirectToAction("UserProfile");
    }
}