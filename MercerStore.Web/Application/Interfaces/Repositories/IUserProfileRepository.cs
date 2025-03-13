using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.Dtos.MetricDto;
using MercerStore.Web.Application.Requests.Users;
using MercerStore.Web.Application.Dtos.UserDto;

namespace MercerStore.Web.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(string id);
        Task<AppUser> GetUserByIdAsyncNoTracking(string id);
        Task<string> GetUserPhotoUrl(string id);
        Task<DateTime> GetUserCreationDate(string userId);
        Task AddUser(AppUser appUser);
        Task UpdateUserProfile(AppUser appUser);
        Task DeleteUserProfile(AppUser appUser);
        Task<(IEnumerable<UserDto>, int totalItems)> GetFilteredUsers(UserFilterRequest request);
        Task<UserMetricDto> GetUserMetric();

    }
}
