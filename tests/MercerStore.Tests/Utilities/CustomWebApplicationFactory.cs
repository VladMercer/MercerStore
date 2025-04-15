using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Infrastructure.Data;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MercerStore.Tests.Utilities;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
            });
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDB"));

            services.AddIdentityCore<AppUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.Configure<JwtOptions>(options =>
            {
                options.SecretKey = "5f8sHurO7DNDylQ2yrWdzdjMrxUX3e7j";
                options.ExpiresDays = 7;
            });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            var userManager = scopedServices.GetRequiredService<UserManager<AppUser>>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            SeedDatabase(userManager, db).GetAwaiter().GetResult();
        });
    }

    private static async Task SeedDatabase(UserManager<AppUser> userManager, AppDbContext db)
    {
        var roles = new List<string> { RoleNames.Admin, RoleNames.User, RoleNames.Manager, RoleNames.Guest };

        foreach (var roleName in roles)
        {
            var existingRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (existingRole == null)
            {
                db.Roles.Add(new AppRole { Name = roleName, NormalizedName = roleName.ToUpper() });
                await db.SaveChangesAsync();
            }
        }

        var testUser = new AppUser
        {
            UserName = "test@example.com",
            Email = "test@example.com"
        };

        var result = await userManager.CreateAsync(testUser, "password123");
        if (!result.Succeeded) throw new Exception("Не удалось создать тестового пользователя");

        var userInDb = await userManager.FindByEmailAsync(testUser.Email);
        var role = await db.Roles.FirstOrDefaultAsync(r => r.Name == RoleNames.User);

        var userRole = new AppUserRole
        {
            UserId = userInDb.Id,
            RoleId = role.Id
        };

        db.UserRoles.Add(userRole);

        await db.SaveChangesAsync();
    }
}