using MediatR;
using MercerStore.Web.Application.Handlers.Account.Commands;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MercerStore.Web.Controllers.Mvc;

public class AccountController : Controller
{
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator, IOptions<JwtOptions> jwtOptions)
    {
        _mediator = mediator;
        _jwtOptions = jwtOptions;
    }

    [HttpGet]
    public IActionResult Login()
    {
        var response = new LoginViewModel();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel, CancellationToken ct)
    {
        var result = await _mediator.Send(new LoginCommand(loginViewModel), ct);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage!);
            return View(loginViewModel);
        }

        AppendJwtTokenToResponse(result.Data);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        var response = new RegisterViewModel();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel, CancellationToken ct)
    {
        var result = await _mediator.Send(new RegisterUserCommand(registerViewModel), ct);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage!);
            return View(registerViewModel);
        }

        AppendJwtTokenToResponse(result.Data!);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult RegisterManager()
    {
        var response = new RegisterViewModel();
        return View(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterManager(RegisterViewModel registerViewModel, CancellationToken ct)
    {
        var result = await _mediator.Send(new RegisterManagerCommand(registerViewModel), ct);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage!);
            return View(registerViewModel);
        }

        AppendJwtTokenToResponse(result.Data);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand());
        return RedirectToAction("Index", "Home");
    }

    private void AppendJwtTokenToResponse(string token)
    {
        Response.Cookies.Append("OohhCookies", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.Value.ExpiresDays)
        });
    }
}