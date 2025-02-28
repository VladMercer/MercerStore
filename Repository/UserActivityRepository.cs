using MercerStore.Data;
using MercerStore.Interfaces;

namespace MercerStore.Repository
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
