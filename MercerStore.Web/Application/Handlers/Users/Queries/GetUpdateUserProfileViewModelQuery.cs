using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Users;

namespace MercerStore.Web.Application.Handlers.Users.Queries;

public record GetUpdateUserProfileViewModelQuery(string UserId) : IRequest<UpdateUserProfileViewModel>;

public class
    GetUpdateUserProfileViewModelHandler : IRequestHandler<GetUpdateUserProfileViewModelQuery,
    UpdateUserProfileViewModel>
{
    private readonly IDateTimeConverter _dateTimeConverter;
    private readonly IUserService _userService;

    public GetUpdateUserProfileViewModelHandler(IUserService userService, IDateTimeConverter dateTimeConverter)
    {
        _userService = userService;
        _dateTimeConverter = dateTimeConverter;
    }

    public async Task<UpdateUserProfileViewModel> Handle(GetUpdateUserProfileViewModelQuery request,
        CancellationToken ct)
    {
        var updateUserProfileViewModel = await _userService.GetUpdateUserProfileViewModel(request.UserId, ct);

        if (updateUserProfileViewModel.CreateDate.HasValue)
            updateUserProfileViewModel.CreateDate =
                _dateTimeConverter.ConvertUtcToUserTime(updateUserProfileViewModel.CreateDate.Value);

        if (updateUserProfileViewModel.LastActivityDate.HasValue)
            updateUserProfileViewModel.LastActivityDate =
                _dateTimeConverter.ConvertUtcToUserTime(updateUserProfileViewModel.LastActivityDate.Value);

        return updateUserProfileViewModel;
    }
}