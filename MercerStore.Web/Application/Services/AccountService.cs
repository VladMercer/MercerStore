using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;

public class AccountService : IAccountService
{
    private const string UnknownRole = "Unknown";
    private const string UserRole = "User";
    private const string ManagerRole = "Manager";
    private const string AuthenticationEntity = "Authentication";
    private const string LoginFailedAction = "Failed login attempt";
    private const string RegisterFailedAction = "Failed register attempt";
    private const string LoginAction = "User logged in";
    private const string RegisterAction = "User registered";
    private const string AdminCreatedManagerAction = "Admin created manager account";

    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserRepository _userProfileRepository;
    private readonly ILogService _logService;
    private readonly IUserIdentifierService _userIdentifierService;
    private readonly double _expiresDays;

    public AccountService(
        UserManager<AppUser> userManager,
        IJwtProvider jwtProvider,
        IUserRepository userProfileRepository,
        ILogService logService,
        IUserIdentifierService userIdentifierService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        _userProfileRepository = userProfileRepository;
        _logService = logService;
        _userIdentifierService = userIdentifierService;
        _expiresDays = configuration.GetValue<double>("JwtOptions:ExpiresDays");
    }

    public async Task<Result<string>> LoginAsync(LoginViewModel loginViewModel, string ipAddress)
    {
        var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
        if (user == null)
        {
            return LogAndReturnFailure(loginViewModel.EmailAddress, "Wrong email address", ipAddress, LoginFailedAction);
        }

        var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
        if (!passwordCheck)
        {
            return LogAndReturnFailure(loginViewModel.EmailAddress, "Invalid password", ipAddress, LoginFailedAction);
        }

        var userId = await _userManager.GetUserIdAsync(user);
        var roles = _userIdentifierService.GetUserRoles(user);
        var profilePictureUrl = await _userProfileRepository.GetUserPhotoUrl(userId);
        var creationDate = await _userProfileRepository.GetUserCreationDate(userId);
        var token = _jwtProvider.GenerateJwtToken(user.Id, roles, profilePictureUrl, creationDate);

        LogSuccessAction(userId, roles, LoginAction, new { token, ipAddress });

        return Result<string>.Success(token);
    }

    public async Task<Result<string>> RegisterUserAsync(RegisterViewModel registerViewModel, string ipAddress)
    {
        return await RegisterUser(registerViewModel, ipAddress, UserRole, RegisterAction);
    }

    public async Task<Result<string>> RegisterManagerAsync(RegisterViewModel registerViewModel, string ipAddress)
    {
        return await RegisterUser(registerViewModel, ipAddress, ManagerRole, AdminCreatedManagerAction, UserRole);
    }

    private async Task<Result<string>> RegisterUser(RegisterViewModel registerViewModel, string ipAddress, string primaryRole, string action, string? additionalRole = null)
    {
        if (await _userManager.FindByEmailAsync(registerViewModel.Email) != null)
        {
            return LogAndReturnFailure(registerViewModel.Email, "Email is already in use", ipAddress, RegisterFailedAction);
        }

        var newUser = new AppUser { Email = registerViewModel.Email, UserName = registerViewModel.Email };
        var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

        if (!result.Succeeded)
        {
            return Result<string>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var roles = new List<string> { primaryRole };
        if (!string.IsNullOrEmpty(additionalRole))
        {
            roles.Add(additionalRole);
        }

        await _userIdentifierService.AddUserToRoleAsync(newUser, primaryRole);
        var token = _jwtProvider.GenerateJwtToken(newUser.Id, roles, null, newUser.DateCreated);

        LogSuccessAction(newUser.Id, roles, action, new { token, ipAddress });

        return Result<string>.Success(token);
    }

    private Result<string> LogAndReturnFailure(string email, string reason, string ipAddress, string action)
    {
        _logService.LogUserAction(
            roles: new List<string> { UnknownRole },
            userId: null,
            action: action,
            entityName: AuthenticationEntity,
            entityId: null,
            details: new { email, reason, ipAddress });

        return Result<string>.Failure(reason);
    }

    private void LogSuccessAction(string userId, List<string> roles, string action, object details)
    {
        _logService.LogUserAction(
            roles: roles,
            userId: userId,
            action: action,
            entityName: "User",
            entityId: null,
            details: details);
    }
}