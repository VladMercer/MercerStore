namespace MercerStore.Web.Application.Interfaces;

public interface IUserIdentifierService
{
    string GetCurrentIdentifier();
    IEnumerable<string> GetCurrentUserRoles();
    List<string> GetUserRoles(string userId);
    Task AddUserToRoleAsync(string userId, List<string> roleNames);
}