using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Dtos.User;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.Requests.Users;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Areas.Admin.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IUserService
{
    Task<PaginatedResultDto<UserDto>> GetFilteredSuppliersWithoutCache(UserFilterRequest request, CancellationToken ct);
    Task<UserProfileViewModel> GetUserProfile(string userId, CancellationToken ct);
    Task<UserProfileViewModel> GetUserProfileForEdit(string userId, CancellationToken ct);
    Task UpdateUserProfile(AppUser user, CancellationToken ct);
    Task<Result<AppUser>> GetUserById(string userId, CancellationToken ct);
    Task<UpdateUserProfileViewModel> GetUpdateUserProfileViewModel(string userId, CancellationToken ct);
}
