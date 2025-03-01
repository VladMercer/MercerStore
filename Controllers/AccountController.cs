using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models.Users;
using MercerStore.Services;
using MercerStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace MercerStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly double _expiresDays;
        private readonly IUserRepository _userProfileRepository;
        private readonly ILogService _logService;
        private readonly IUserIdentifierService _userIdentifierService;

        private const string UnknownRole = "Unknown";
        private const string UserRole = "User";
        private const string ManagerRole = "Manager";
        private const string AuthenticationEntity = "Authentication";
        private const string LoginFailedAction = "Failed login attempt";
        private const string RegisterFailedAction = "Failed register attempt";

        public AccountController(UserManager<AppUser> userManager, IJwtProvider jwtProvider, IConfiguration configuration, IUserRepository userProfileRepository, ILogService logService, IUserIdentifierService userIdentifierService)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _expiresDays = configuration.GetValue<double>("JwtOptions:ExpiresDays");
            _userProfileRepository = userProfileRepository;
            _logService = logService;
            _userIdentifierService = userIdentifierService;
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

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                LogFailedAction(loginViewModel.EmailAddress, $"Invalid model state: {string.Join(", ", errors)}", LoginFailedAction, ipAddress);
                return View(loginViewModel);
            }

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {

                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (passwordCheck)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var roles = _userIdentifierService.GetUserRoles(user);
                    var profilePictureUrl = await _userProfileRepository.GetUserPhotoUrl(userId);
                    var creationDate = await _userProfileRepository.GetUserCreationDate(userId);

                    var token = _jwtProvider.GenerateJwtToken(user.Id, roles, profilePictureUrl, creationDate);

                    AppendJwtTokenToResponse(token);

                    _logService.LogUserAction(
                        roles: roles,
                        userId: userId,
                        action: "User logged in",
                        entityName: UserRole,
                        entityId: null,
                        details: new
                        {
                            token,
                            ipAddress
                        }
                    );
                    return RedirectToAction("Index", "Home");
                }

                LogFailedAction(loginViewModel.EmailAddress, "Invalid password", LoginFailedAction, ipAddress);
                return View(loginViewModel);
            }

            LogFailedAction(loginViewModel.EmailAddress, "Wrong email address", LoginFailedAction, ipAddress);
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
            var ipAddress = GetClientIpAddress();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                LogFailedAction(registerViewModel.Email, $"Invalid model state: {string.Join(", ", errors)}", RegisterFailedAction, ipAddress);
                return View(registerViewModel);
            }

            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (user != null)
            {
                LogFailedAction(registerViewModel.Email, "Email is already use", RegisterFailedAction, ipAddress);
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
                await _userIdentifierService.AddUserToRoleAsync(newUser, UserRole);
                var userId = await _userManager.GetUserIdAsync(newUser);
                var roles = new List<string> { UserRole };

                var token = _jwtProvider.GenerateJwtToken(userId, roles, null, newUser.DateCreated);

                AppendJwtTokenToResponse(token);

                _logService.LogUserAction(
                      roles: roles,
                      userId: userId,
                      action: "User has registered",
                      entityName: UserRole,
                      entityId: null,
                      details: new
                      {
                          token,
                          ipAddress
                      }
                  );

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in newUserResponse.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(registerViewModel);
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

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                LogFailedAction(registerViewModel.Email, $"Invalid model state: {string.Join(", ", errors)}", "Failed manager registration", ipAddress);
                return View(registerViewModel);
            }

            var existingUser = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (existingUser != null)
            {
                LogFailedAction(registerViewModel.Email, "Email is already in use", "Failed manager registration", ipAddress);
                ModelState.AddModelError("", "Email is already in use.");
                return View(registerViewModel);
            }

            var newManager = new AppUser()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email
            };

            var createUserResult = await _userManager.CreateAsync(newManager, registerViewModel.Password);

            if (createUserResult.Succeeded)
            {
                await _userIdentifierService.AddUserToRoleAsync(newManager, ManagerRole); 

                var userId = await _userManager.GetUserIdAsync(newManager);
                var roles = new List<string> { ManagerRole, UserRole };

                var token = _jwtProvider.GenerateJwtToken(userId, roles, null, newManager.DateCreated);
                AppendJwtTokenToResponse(token);

                _logService.LogUserAction(
                    roles: roles,
                    userId: userId,
                    action: "Admin created manager account",
                    entityName: ManagerRole,
                    entityId: null,
                    details: new
                    {
                        token,
                        ipAddress
                    }
                );

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in createUserResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(registerViewModel);
        }

        [HttpGet]
        [LogUserAction("User logged out", UserRole)]
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
        private void LogFailedAction(string email, string reason, string action, string? ipAddress)
        {
            _logService.LogUserAction(
                roles: [UserRole],
                userId: null,
                action: action,
                entityName: AuthenticationEntity,
                entityId: null,
                details: new
                {
                    email,
                    reason,
                    ipAddress
                }
            );
        }
        private string? GetClientIpAddress()
        {
            return Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }
}

