﻿using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Account;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Account.Commands;

public record LoginCommand(LoginViewModel LoginViewModel) :
    LoggableRequest<Result<string>>("User logged in", "User");

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
{
    private readonly IAccountService _accountService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserIdentifierService _userIdentifierService;

    public LoginCommandHandler(
        IJwtProvider jwtProvider,
        IUserIdentifierService userIdentifierService,
        IAccountService accountService)
    {
        _jwtProvider = jwtProvider;
        _userIdentifierService = userIdentifierService;
        _accountService = accountService;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken ct)
    {
        var result = await _accountService.LoginAsync(request.LoginViewModel, ct).ConfigureAwait(true);

        if (!result.IsSuccess)
        {
            request.Details = new { result.Data, result.ErrorMessage };
            return Result<string>.Failure(result.ErrorMessage);
        }

        request.EntityId = result.Data.UserId;
        var roles = await _userIdentifierService.GetUserRoles(result.Data.UserId, ct);
        var timeZone = _userIdentifierService.GetCurrentUserTimeZone();

        var jwtTokenRequest = new JwtTokenRequest(result.Data.UserId, roles, result.Data.ProfilePictureUrl,
            result.Data.CreateDate, timeZone);

        var token = await _jwtProvider.GenerateJwtToken(jwtTokenRequest).ConfigureAwait(true);

        request.Details = new { result.Data, token, result.ErrorMessage };

        return Result<string>.Success(token);
    }
}
