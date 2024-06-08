using MercerStore.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MercerStore.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<AppUser> GetUserByIdAsync(string id);
        Task<AppUser> GetUserByIdAsyncNoTracking(string id);
        bool Add(AppUser appUser);
        bool Update(AppUser appUser);
        bool Delete(AppUser appUser);
        bool Save();

    }
}
