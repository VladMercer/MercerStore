using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MercerStore.Web.Controllers.Mvc
{
    public class AccountController : Controller
    {
       private readonly IAccountService _accountService;
       private readonly double _expiresDays;
        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _expiresDays = double.Parse(configuration["JwtOptions:ExpiresDays"]);
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var ipAddress = GetClientIpAddress();
            var result = await _accountService.LoginAsync(loginViewModel, ipAddress);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage!);
                return View(loginViewModel);
            }

            AppendJwtTokenToResponse(result.Data!);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var ipAddress = GetClientIpAddress();
            var result = await _accountService.RegisterUserAsync(registerViewModel, ipAddress);

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
        public async Task<IActionResult> RegisterManager(RegisterViewModel registerViewModel)
        {
            var ipAddress = GetClientIpAddress();
            var result = await _accountService.RegisterManagerAsync(registerViewModel, ipAddress);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage!);
                return View(registerViewModel);
            }

            AppendJwtTokenToResponse(result.Data);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [LogUserAction("User logged out", "User")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("OohhCookies");
            return RedirectToAction("Index", "Home");
        }
        private void AppendJwtTokenToResponse(string token)
        {
            Response.Cookies.Append("OohhCookies", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(_expiresDays)
            });
        }
        private string? GetClientIpAddress()
        {
            return Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }
}

