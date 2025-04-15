using MediatR;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Users;

namespace MercerStore.Web.Application.Handlers.Users.Queries
{
    public record GetUserProfileForEditQuery() : IRequest<UserProfileViewModel>;
    public class GetUserProfileForEditHandler : IRequestHandler<GetUserProfileForEditQuery, UserProfileViewModel>
    {
        private readonly IUserService _userService;
        private readonly IUserIdentifierService _userIdentifierService;

        public GetUserProfileForEditHandler(IUserService userService, IUserIdentifierService userIdentifierService)
        {
            _userService = userService;
            _userIdentifierService = userIdentifierService;
        }

        public async Task<UserProfileViewModel> Handle(GetUserProfileForEditQuery request, CancellationToken cancellationToken)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            return await _userService.GetUserProfileForEdit(userId);
        }
    }
}
