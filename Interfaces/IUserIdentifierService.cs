using MercerStore.Models.Users;

namespace MercerStore.Interfaces
{
    public interface IUserIdentifierService
    {
        string GetCurrentIdentifier();
        IEnumerable<string> GetCurrentUserRoles();
        List<string> GetUserRoles(AppUser user);
        Task<bool> AddUserToRoleAsync(AppUser user, string roleName);
    }
}
