using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Account;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Account.Commands;

public record RegisterUserCommand(RegisterViewModel RegisterViewModel) :
    LoggableRequest<Result<string>>("User registered", "User");

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<string>>
{
    private readonly IAccountService _accountService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserIdentifierService _userIdentifierService;

    public RegisterUserHandler(
        IJwtProvider jwtProvider,
        ILogService logService,
        IAccountService accountService, IUserIdentifierService userIdentifierService)
    {
        _jwtProvider = jwtProvider;
        _accountService = accountService;
        _userIdentifierService = userIdentifierService;
    }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        var result = await _accountService.RegisterUserAsync(request.RegisterViewModel);
        request.EntityId = result.Data.UserId;

        if (!result.IsSuccess)
        {
            request.Details = new { result.Data, result.ErrorMessage };
            return Result<string>.Failure(result.ErrorMessage);
        }

        var roles = new List<string> { RoleNames.User };
        await _userIdentifierService.AddUserToRoleAsync(result.Data.UserId, roles, ct);
        var timeZone = _userIdentifierService.GetCurrentUserTimeZone();

        var jwtTokenRequest = new JwtTokenRequest(result.Data.UserId, roles, result.Data.ProfilePictureUrl,
            result.Data.CreateDate, timeZone);

        var token = await _jwtProvider.GenerateJwtToken(jwtTokenRequest);

        request.Details = new { result.Data, token };

        return Result<string>.Success(token);
    }
}