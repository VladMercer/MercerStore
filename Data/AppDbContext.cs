using MercerStore.Models;
using MercerStore.Models.DescriptionProducts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<CaseDetail> CaseDetails { get; set; }
        public DbSet<CoolingSystemDetail> CoolingSystemDetails { get; set; }
        public DbSet<MotherboardDetail> MotherboardDetails { get; set; }
        public DbSet<PowerSupplyDetail> PowerSupplyDetails { get; set; }
        public DbSet<ProcessorDetail> ProcessorDetails { get; set; }
        public DbSet<RamDetail> RamDetail { get; set; }
        public DbSet<StorageDetail> StorageDetail { get; set; }
        public DbSet<VideoCardDetail> VideoCardDetails { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public static void Seed(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)

        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole { Name = "Admin" };
                roleManager.CreateAsync(role).Wait();
            }

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                var role = new IdentityRole { Name = "User" };
                roleManager.CreateAsync(role).Wait();
            }

            if (userManager.FindByEmailAsync("invoker.sama.vlad@gmail.com").Result == null)
            {
                var user = new AppUser
                {
                    UserName = "Mercer",
                    Email = "invoker.sama.vlad@gmail.com"
                };

                var result = userManager.CreateAsync(user, "admin1234").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
