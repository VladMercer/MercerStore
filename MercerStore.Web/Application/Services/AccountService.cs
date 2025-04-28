using MercerStore.Web.Application.Dtos.Account;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;

namespace MercerStore.Web.Application.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserRepository _userProfileRepository;

    public AccountService(UserManager<AppUser> userManager, IUserRepository userProfileRepository)
    {
        _userManager = userManager;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<Result<JwtTokenDto>> LoginAsync(LoginViewModel loginViewModel, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

        if (user == null) return Result<JwtTokenDto>.Failure("Wrong email address");

        var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
        if (!passwordCheck) return Result<JwtTokenDto>.Failure("Invalid password");

        var userId = await _userManager.GetUserIdAsync(user);
        var profilePictureUrl = await _userProfileRepository.GetUserPhotoUrl(userId, ct);
        var creationDate = await _userProfileRepository.GetUserCreationDate(userId, ct);

        var jwtTokenDto = new JwtTokenDto
        {
            UserId = userId,
            ProfilePictureUrl = profilePictureUrl,
            CreateDate = creationDate
        };

        return Result<JwtTokenDto>.Success(jwtTokenDto);
    }

    public async Task<Result<JwtTokenDto>> RegisterUserAsync(RegisterViewModel registerViewModel)
    {
        return await RegisterUser(registerViewModel);
    }

    public async Task<Result<JwtTokenDto>> RegisterManagerAsync(RegisterViewModel registerViewModel)
    {
        return await RegisterUser(registerViewModel);
    }

    private async Task<Result<JwtTokenDto>> RegisterUser(RegisterViewModel registerViewModel)
    {
        if (await _userManager.FindByEmailAsync(registerViewModel.Email) != null)
            return Result<JwtTokenDto>.Failure("Email is already in use");

        var newUser = new AppUser { Email = registerViewModel.Email, UserName = registerViewModel.Email };
        var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

        if (!result.Succeeded)
            return Result<JwtTokenDto>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

        var jwtTokenDto = new JwtTokenDto
        {
            UserId = newUser.Id,
            ProfilePictureUrl = null,
            CreateDate = newUser.DateCreated
        };

        return Result<JwtTokenDto>.Success(jwtTokenDto);
    }
}