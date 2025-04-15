using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Users;

namespace MercerStore.Web.Application.Handlers.Users.Queries
{
    public record GetUpdateUserProfileViewModelQuery(string UserId) : IRequest<UpdateUserProfileViewModel>;
    public class GetUpdateUserProfileViewModelHandler : IRequestHandler<GetUpdateUserProfileViewModelQuery, UpdateUserProfileViewModel>
    {
        private readonly IUserService _userService;

        public GetUpdateUserProfileViewModelHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdateUserProfileViewModel> Handle(GetUpdateUserProfileViewModelQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUpdateUserProfileViewModel(request.UserId);
        }
    }
}
