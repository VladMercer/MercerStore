using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Users;

namespace MercerStore.Web.Application.Handlers.Users.Queries;

public record GetUserProfileQuery : IRequest<UserProfileViewModel>;

public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, UserProfileViewModel>
{
    private readonly IUserIdentifierService _userIdentifierService;
    private readonly IUserService _userService;

    public GetUserProfileHandler(IUserService userService, IUserIdentifierService userIdentifierService)
    {
        _userService = userService;
        _userIdentifierService = userIdentifierService;
    }

    public async Task<UserProfileViewModel> Handle(GetUserProfileQuery request, CancellationToken ct)
    {
        var userId = _userIdentifierService.GetCurrentIdentifier();
        return await _userService.GetUserProfile(userId, ct);
    }
}
