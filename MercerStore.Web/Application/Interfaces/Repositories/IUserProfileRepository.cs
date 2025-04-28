using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Dtos.User;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.Requests.Users;

namespace MercerStore.Web.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<AppUser> GetUserByIdAsync(string id, CancellationToken ct);
    Task<AppUser> GetUserByIdAsyncNoTracking(string id, CancellationToken ct);
    Task<string> GetUserPhotoUrl(string id, CancellationToken ct);
    Task<DateTime> GetUserCreationDate(string userId, CancellationToken ct);
    Task AddUser(AppUser appUser, CancellationToken ct);
    Task UpdateUserProfile(AppUser appUser, CancellationToken ct);
    Task DeleteUserProfile(AppUser appUser, CancellationToken ct);

    Task<(IEnumerable<UserDto>, int totalItems)> GetFilteredUsers(UserFilterRequest request,
        CancellationToken ct);

    Task<UserMetricDto> GetUserMetric(CancellationToken ct);
}