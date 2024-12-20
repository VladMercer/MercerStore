using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace RunGroopWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly double _expiresDays;


        public AccountController(UserManager<AppUser> userManager, IJwtProvider jwtProvider, IConfiguration configuration)
        {

            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _expiresDays = double.Parse(configuration["JwtOptions:ExpiresDays"]);
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

        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);


            if (user != null)
            {

                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {


                    var roles = new List<string> { "User" };
                    var token = _jwtProvider.GenerateJwtToken(user.Id, roles);
                    AppendJwtTokenToResponse(token);

                    return RedirectToAction("Index", "Home");
                }

                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }

            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
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
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (user != null)
            {
                TempData["Error"] = "Этот email уже используется";
                return View(registerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                var role = "User";

                await _userManager.AddToRoleAsync(newUser, role);

				var roles = new List<string> { role };

				var token = _jwtProvider.GenerateJwtToken(user.Id, roles);

                AppendJwtTokenToResponse(token);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in newUserResponse.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("OohhCookies");
            return RedirectToAction("Index", "Home");

        }
    }

}

