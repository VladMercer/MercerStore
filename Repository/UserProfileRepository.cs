using MercerStore.Data;
using MercerStore.Interfaces;
using MercerStore.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Repository
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public bool Add(AppUser appUser)
        {
            _context.Add(appUser);
            return Save();
        }

        public bool Delete(AppUser appUser)
        {
            _context.Remove(appUser);
            return Save();
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByIdAsyncNoTracking(string id)
        {
            return await _context.Users.Where(i => i.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser appUser)
        {
            _context.Users.Update(appUser);
            return Save();
        }
    }
}
