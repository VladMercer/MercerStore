using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.UserDto;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.Requests.Users;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Areas.Admin.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<PaginatedResultDto<UserDto>> GetFilteredSuppliersWithoutCache(UserFilterRequest request);
        Task<UserProfileViewModel> GetUserProfile(string userId);
        Task<UserProfileViewModel> GetUserProfileForEdit(string userId);
        Task UpdateUserProfile(AppUser user);
        Task<Result<AppUser>> GetUserById(string userId);
        Task<UpdateUserProfileViewModel> GetUpdateUserProfileViewModel(string userId);
    }
}
