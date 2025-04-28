using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Dtos.User;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.Requests.Users;
using MercerStore.Web.Infrastructure.Data;
using MercerStore.Web.Infrastructure.Data.Enum;
using MercerStore.Web.Infrastructure.Data.Enum.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public Task DeleteUserProfile(AppUser appUser, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<AppUser> GetUserByIdAsync(string id, CancellationToken ct)
    {
        return await _context.Users
            .Include(u => u.Orders)
            .Include(u => u.Reviews)
            .ThenInclude(r => r.Product)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<AppUser> GetUserByIdAsyncNoTracking(string id, CancellationToken ct)
    {
        return await _context.Users.Where(i => i.Id == id).AsNoTracking().FirstOrDefaultAsync(ct);
    }

    public async Task<string> GetUserPhotoUrl(string id, CancellationToken ct)
    {
        return await _context.AppUsers.Where(u => u.Id == id).Select(u => u.UserImgUrl)
            .FirstOrDefaultAsync(ct);
    }

    public async Task UpdateUserProfile(AppUser appUser, CancellationToken ct)
    {
        _context.Users.Update(appUser);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<DateTime> GetUserCreationDate(string userId, CancellationToken ct)
    {
        return await _context.AppUsers.Where(u => u.Id == userId).Select(u => u.DateCreated)
            .FirstOrDefaultAsync(ct);
    }

    public async Task AddUser(AppUser appUser, CancellationToken ct)
    {
        await _context.Users.AddAsync(appUser, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<(IEnumerable<UserDto>, int totalItems)> GetFilteredUsers(UserFilterRequest request,
        CancellationToken ct)
    {
        var currentDay = DateTime.UtcNow;
        var usersQuery = _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(u => u.Orders)
            .Include(u => u.Reviews)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Query))
            usersQuery = usersQuery.Where(u =>
                u.Id == request.Query ||
                EF.Functions.ILike(u.Email, $"%{request.Query}%") ||
                EF.Functions.ILike(u.Address, $"%{request.Query}%")
            );

        if (request.Period.HasValue && request.Period != TimePeriod.All)
        {
            var filterDate = request.Period switch
            {
                TimePeriod.Day => currentDay.AddDays(-1),
                TimePeriod.Week => currentDay.AddDays(-7),
                TimePeriod.Month => currentDay.AddMonths(-1),
                TimePeriod.Quarter => currentDay.AddMonths(-3),
                TimePeriod.Year => currentDay.AddYears(-1),
                _ => currentDay
            };

            usersQuery = request.Filter switch
            {
                UserFilter.LastActivityDate => usersQuery.Where(o => o.LastActivity >= filterDate),
                UserFilter.CreateDate => usersQuery.Where(o => o.DateCreated >= filterDate),
                _ => usersQuery
            };
        }

        if (request.Filter.HasValue)
        {
            var roleFilters = new HashSet<UserFilter>
            {
                UserFilter.Admin,
                UserFilter.Banned,
                UserFilter.Manager,
                UserFilter.User
            };

            Dictionary<string, string>? roleIds = null;
            if (request.Filter.HasValue && roleFilters.Contains(request.Filter.Value))
                roleIds = await _context.Roles.ToDictionaryAsync(r => r.Name, r => r.Id, ct);

            usersQuery = request.Filter switch
            {
                _ when roleIds is not null => request.Filter.Value switch
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
                UserFilter.Offline => usersQuery.Where(o => !(o.LastActivity >= currentDay.AddMinutes(-5))),
                UserFilter.Online => usersQuery.Where(o => o.LastActivity >= currentDay.AddMinutes(-5)),
                _ => usersQuery
            };
        }

        var groupedQuery = usersQuery.Select(u => new
        {
            User = u,
            OrdersCount = u.Orders.Count(),
            ReviewsCount = u.Reviews.Count()
        });

        groupedQuery = request.SortOrder switch
        {
            UserSortOrder.NameAsc => groupedQuery.OrderBy(p => p.User.Email),
            UserSortOrder.NameDesc => groupedQuery.OrderByDescending(p => p.User.Email),
            UserSortOrder.Online => groupedQuery
                .Where(p => p.User.LastActivity >= currentDay.AddMinutes(-5))
                .OrderBy(p => p.User.LastActivity),
            UserSortOrder.Offline => groupedQuery
                .Where(p => p.User.LastActivity <= currentDay.AddMinutes(-5))
                .OrderBy(p => p.User.LastActivity),
            UserSortOrder.LastActivityDateAsc => groupedQuery.OrderBy(p => p.User.LastActivity),
            UserSortOrder.LastActivityDateDesc => groupedQuery.OrderByDescending(p => p.User.LastActivity),
            UserSortOrder.CreateDateAsc => groupedQuery.OrderBy(p => p.User.DateCreated),
            UserSortOrder.CreateDateDesc => groupedQuery.OrderByDescending(p => p.User.DateCreated),
            _ => groupedQuery.OrderBy(p => p.User.DateCreated)
        };

        var totalItems = await groupedQuery.CountAsync(ct);

        var users = await groupedQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

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

    public async Task<UserMetricDto> GetUserMetric(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var dayOfWeek = (int)now.DayOfWeek;
        dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
        var startOfWeek = now.Date.AddDays(1 - dayOfWeek);
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
            .ToDictionaryAsync(r => r.Name, r => r.Id, ct);

        var users = usersQuery.Where(u => u.UserRoles
                .Any(ur => ur.RoleId == roleIds["User"]))
            .GroupBy(o => o.LastActivity.Value.Date)
            .Select(g => new
            {
                Date = g.Key,
                Users = g.Count()
            });

        var userMetric = new UserMetricDto
        {
            Total = users.Count(),
            NewUsers = new NewUsersDto
            {
                Daily = users.Where(x => x.Date == now.Date).Sum(x => x.Users),
                Weekly = users.Where(x => x.Date >= startOfWeek).Sum(x => x.Users),
                Monthly = users.Where(x => x.Date >= startOfMonth).Sum(x => x.Users),
                Yearly = users.Where(x => x.Date >= startOfYear).Sum(x => x.Users)
            }
        };
        return userMetric;
    }
}