using MercerStore.Data.Enum.User;
using MercerStore.Data.Enum;
using MercerStore.Dtos.UserDto;
using MercerStore.Models.Users;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MercerStore.Interfaces
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
        Task<(IEnumerable<UserDto>, int totalItems)> GetFilteredUsers(
          int pageNumber,
          int pageSize,
          UserSortOrder? sortOrder,
          TimePeriod? timePeriod,
          UserFilter? filter,
          string? query);
        Task<object> GetUserMetric();

    }
}
