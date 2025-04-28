using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Infrastructure.Data;
using MercerStore.Web.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Services;

public class UserIdentifierService : IUserIdentifierService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdentifierService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public string GetCurrentIdentifier()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.GetUserId();
    }

    public IEnumerable<string> GetCurrentUserRoles()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.GetUserRoles();
    }

    public async Task<IList<string>> GetUserRoles(string userId, CancellationToken ct)
    {
        var roles = await _context.UserRoles
            .AsNoTracking()
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .ToListAsync(ct);
        return roles;
    }

    public async Task AddUserToRoleAsync(string userId, IList<string> roleNames, CancellationToken ct)
    {
        foreach (var roleName in roleNames)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct);

            var userRole = new AppUserRole { UserId = userId, RoleId = role.Id };
            await _context.Set<AppUserRole>().AddAsync(userRole, ct);
            await _context.SaveChangesAsync(ct);
        }
    }

    public string? GetCurrentUserTimeZone()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.GetUserTimeZone() ?? "UTC";
    }
}
