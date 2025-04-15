using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Infrastructure.Data;
using MercerStore.Web.Infrastructure.Extentions;
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

    public List<string> GetUserRoles(string userId)
    {
        var roles = _context.UserRoles
            .AsNoTracking()
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .ToList();
        return roles;
    }

    public async Task AddUserToRoleAsync(string userId, List<string> roleNames)
    {
        foreach (var roleName in roleNames)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            var userRole = new AppUserRole { UserId = userId, RoleId = role.Id };
            await _context.Set<AppUserRole>().AddAsync(userRole);
            await _context.SaveChangesAsync();
        }
    }
}