using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Infrastructure.Data;

namespace MercerStore.Web.Infrastructure.Repositories
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly AppDbContext _context;

        public UserActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateLastActivityAsync(string userId, DateTime lastActivity)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastActivity = lastActivity;
                await _context.SaveChangesAsync();
            }
        }
    }
}
