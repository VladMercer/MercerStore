using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Infrastructure.Data;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Services
{
    public class UserIdentifierService : IUserIdentifierService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

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
        public List<string> GetUserRoles(AppUser user)
        {
            var roles = _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name)
                .ToList();

            return roles;
        }
        public async Task<bool> AddUserToRoleAsync(AppUser user, string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null) return false;

            var userRole = new AppUserRole { UserId = user.Id, RoleId = role.Id };
            await _context.Set<AppUserRole>().AddAsync(userRole);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
