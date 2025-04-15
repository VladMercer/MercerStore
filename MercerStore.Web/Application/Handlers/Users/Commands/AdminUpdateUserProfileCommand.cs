using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Areas.Admin.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Users.Commands
{
    public record AdminUpdateUserProfileCommand(UpdateUserProfileViewModel UpdateUserProfileViewModel) :
        LoggableRequest<Result<Unit>>("Manager update user profile", "user");
    public class AdminUpdateUserProfileHandler : IRequestHandler<AdminUpdateUserProfileCommand, Result<Unit>>
    {
        private readonly IUserService _userService;

        public AdminUpdateUserProfileHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<Unit>> Handle(AdminUpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.GetUserById(request.UpdateUserProfileViewModel.Id);
            if (!result.IsSuccess)
            {
                return Result<Unit>.Failure(result.ErrorMessage);
            }
            var user = result.Data;

            user.UserImgUrl = request.UpdateUserProfileViewModel.UserImgUrl ?? "https://localhost:7208/img/default/default_user_image.jpg";
            user.PhoneNumber = request.UpdateUserProfileViewModel.PhoneNumber;
            user.Address = request.UpdateUserProfileViewModel.Address;
            user.Email = request.UpdateUserProfileViewModel.EmailAddress;
            user.UserName = request.UpdateUserProfileViewModel.UserName;

            await _userService.UpdateUserProfile(user);

            var logDetails = new
            {
                user.UserImgUrl,
                user.PhoneNumber,
                user.Address,
                user.Email,
                user.UserName
            };

            request.EntityId = user.Id;
            request.Details = logDetails;

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
