using MercerStore.Data;
using MercerStore.Data.Enum;
using MercerStore.Data.Enum.User;
using MercerStore.Dtos.UserDto;
using MercerStore.Interfaces;
using MercerStore.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public Task DeleteUserProfile(AppUser appUser)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _context.Users
             .Include(u => u.Orders)
             .Include(u => u.Reviews)
             .ThenInclude(r => r.Product)
             .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
             .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppUser> GetUserByIdAsyncNoTracking(string id)
        {
            return await _context.Users.Where(i => i.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<string> GetUserPhotoUrl(string id)
        {
            return await _context.AppUsers.Where(u => u.Id == id).Select(u => u.UserImgUrl).FirstOrDefaultAsync();
        }

        public async Task UpdateUserProfile(AppUser appUser)
        {
            _context.Users.Update(appUser);
            await _context.SaveChangesAsync();
        }
        public async Task<DateTime> GetUserCreationDate(string userId)
        {
            return await _context.AppUsers.Where(u => u.Id == userId).Select(u => u.DateCreated).FirstOrDefaultAsync();
        }
        public async Task AddUser(AppUser appUser)
        {
            await _context.Users.AddAsync(appUser);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<UserDto>, int totalItems)> GetFilteredUsers(
          int pageNumber,
          int pageSize,
          UserSortOrder? sortOrder,
          TimePeriod? timePeriod,
          UserFilter? filter,
          string? query)
        {

            var currentDay = DateTime.UtcNow;
            var usersQuery = _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.Orders)
                .Include(u => u.Reviews)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                usersQuery = usersQuery.Where(u =>
                    u.Id == query ||
                    EF.Functions.ILike(u.Email, $"%{query}%") ||
                    EF.Functions.ILike(u.Address, $"%{query}%")
                );
            }

            if (timePeriod.HasValue && timePeriod != TimePeriod.All)
            {
                var filterDate = timePeriod switch
                {
                    TimePeriod.Day => currentDay.AddDays(-1),
                    TimePeriod.Week => currentDay.AddDays(-7),
                    TimePeriod.Month => currentDay.AddMonths(-1),
                    TimePeriod.Quarter => currentDay.AddMonths(-3),
                    TimePeriod.Year => currentDay.AddYears(-1),
                    _ => currentDay,
                };

                usersQuery = filter switch
                {
                    UserFilter.LastActivityDate => usersQuery.Where(o => o.LastActivity >= filterDate),
                    UserFilter.CreateDate => usersQuery.Where(o => o.DateCreated >= filterDate),
                    _ => usersQuery,
                };
            }

            if (filter.HasValue)
            {
                var roleFilters = new HashSet<UserFilter>
                {
                    UserFilter.Admin,
                    UserFilter.Banned,
                    UserFilter.Manager,
                    UserFilter.User
                };

                Dictionary<string, string>? roleIds = null;
                if (filter.HasValue && roleFilters.Contains(filter.Value))
                {
                    roleIds = await _context.Roles.ToDictionaryAsync(r => r.Name, r => r.Id);
                }

                usersQuery = filter switch
                {
                    _ when roleIds is not null => filter.Value switch
                    {
                        UserFilter.Admin => usersQuery.Where(u => u.UserRoles.Any(ur => ur.RoleId == roleIds["Admin"])),
                        UserFilter.Banned => usersQuery.Where(u => u.UserRoles.Any(ur => ur.RoleId == roleIds["Banned"])),
                        UserFilter.Manager => usersQuery.Where(u => u.UserRoles.Any(ur => ur.RoleId == roleIds["Manager"])),
                        UserFilter.User => usersQuery.Where(u => u.UserRoles.Any(ur => ur.RoleId == roleIds["User"])),
                        _ => usersQuery
                    },

                    UserFilter.HasOrder => usersQuery.Where(u => u.Orders.Any()),
                    UserFilter.NoOrder => usersQuery.Where(u => !u.Orders.Any()),
                    UserFilter.HasReview => usersQuery.Where(u => u.Reviews.Any()),
                    UserFilter.NoReview => usersQuery.Where(u => !u.Reviews.Any()),
                    UserFilter.Ofline => usersQuery.Where(o => !(o.LastActivity >= currentDay.AddMinutes(-5))),
                    UserFilter.Online => usersQuery.Where(o => o.LastActivity >= currentDay.AddMinutes(-5)),
                    _ => usersQuery,
                };
            }

            var groupedQuery = usersQuery.Select(u => new
            {
                User = u,
                OrdersCount = u.Orders.Count(),
                ReviewsCount = u.Reviews.Count()
            });

            groupedQuery = sortOrder switch
            {
                UserSortOrder.NameAsc => groupedQuery.OrderBy(p => p.User.Email),
                UserSortOrder.NameDesc => groupedQuery.OrderByDescending(p => p.User.Email),
                UserSortOrder.Online => groupedQuery
                .Where(p => p.User.LastActivity >= currentDay.AddMinutes(-5))
                .OrderBy(p => p.User.LastActivity),
                UserSortOrder.Ofline => groupedQuery
                .Where(p => p.User.LastActivity <= currentDay.AddMinutes(-5))
                .OrderBy(p => p.User.LastActivity),
                UserSortOrder.LastActivityDateAsc => groupedQuery.OrderBy(p => p.User.LastActivity),
                UserSortOrder.LastActivityDateDesc => groupedQuery.OrderByDescending(p => p.User.LastActivity),
                UserSortOrder.CreateDateAsc => groupedQuery.OrderBy(p => p.User.DateCreated),
                UserSortOrder.CreateDateDesc => groupedQuery.OrderByDescending(p => p.User.DateCreated),
                _ => groupedQuery.OrderBy(p => p.User.DateCreated),
            };

            var totalItems = await groupedQuery.CountAsync();

            var users = await groupedQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = users.Select(u => new UserDto
            {
                Id = u.User.Id,
                PhoneNumber = u.User.PhoneNumber,
                ImageUrl = u.User.UserImgUrl,
                CountOrders = u.OrdersCount,
                CountReviews = u.ReviewsCount,
                LastActivityDate = u.User.LastActivity,
                Email = u.User.Email,
                Address = u.User.Address,
                CreateDate = u.User.DateCreated,
                Roles = u.User.UserRoles.Select(ur => ur.Role.Name).ToList()
            }).ToList();

            return (userDtos, totalItems);
        }
        public async Task<object> GetUserMetric()
        {
            var now = DateTime.UtcNow;
            var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + 1);
            var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var startOfYear = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            Dictionary<string, string>? roleIds = null;

            var usersQuery = _context.Users
                .AsNoTracking()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.Orders)
                .Include(u => u.Reviews)
                .AsQueryable();

            roleIds = await _context.Roles
                .AsNoTracking()
                .ToDictionaryAsync(r => r.Name, r => r.Id);

            var users = usersQuery.Where(u => u.UserRoles
            .Any(ur => ur.RoleId == roleIds["User"]))
            .GroupBy(o => o.LastActivity.Value.Date)
            .Select(g => new
            {
                Date = g.Key,
                Users = g.Count()
            });

            var userMetric = new
            {
                Total = users.Count(),
                NewUsers = new
                {
                    Daily = users.Where(x => x.Date == now.Date).Sum(x => x.Users),
                    Weekly = users.Where(x => x.Date >= startOfWeek).Sum(x => x.Users),
                    Monthly = users.Where(x => x.Date >= startOfMonth).Sum(x => x.Users),
                    Yearly = users.Where(x => x.Date >= startOfYear).Sum(x => x.Users)
                },

            };
            return userMetric;
        }
    }
}
