using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Users;

namespace MercerStore.Web.Application.Handlers.Users.Queries;

public record GetUserProfileForEditQuery : IRequest<UserProfileViewModel>;

public class GetUserProfileForEditHandler : IRequestHandler<GetUserProfileForEditQuery, UserProfileViewModel>
{
    private readonly IUserIdentifierService _userIdentifierService;
    private readonly IUserService _userService;

    public GetUserProfileForEditHandler(IUserService userService, IUserIdentifierService userIdentifierService)
    {
        _userService = userService;
        _userIdentifierService = userIdentifierService;
    }

    public async Task<UserProfileViewModel> Handle(GetUserProfileForEditQuery request,
        CancellationToken ct)
    {
        var userId = _userIdentifierService.GetCurrentIdentifier();
        return await _userService.GetUserProfileForEdit(userId, ct);
    }
}