namespace MercerStore.Web.Application.Interfaces.Services;

public interface IUserIdentifierService
{
    string GetCurrentIdentifier();
    IEnumerable<string> GetCurrentUserRoles();
    Task<IList<string>> GetUserRoles(string userId, CancellationToken ct);
    Task AddUserToRoleAsync(string userId, IList<string> roleNames, CancellationToken ct);
    string? GetCurrentUserTimeZone();
}