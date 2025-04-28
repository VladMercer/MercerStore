using MercerStore.Web.Application.Models.Carts;
using MercerStore.Web.Application.Models.Invoices;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Models.sales;
using MercerStore.Web.Application.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string,
    IdentityUserClaim<string>, AppUserRole,
    IdentityUserLogin<string>, IdentityRoleClaim<string>,
    IdentityUserToken<string>>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderProductSnapshot> OrderProductSnapshots { get; set; }
    public virtual DbSet<OrderProductList> OrderProductLists { get; set; }
    public virtual DbSet<ProductStatus> ProductStatuses { get; set; }
    public virtual DbSet<ProductDescription> ProductDescriptions { get; set; }
    public virtual DbSet<ProductPricing> ProductPricings { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<ProductImage> ProductImages { get; set; }
    public virtual DbSet<ProductVariant> ProductVariants { get; set; }
    public virtual DbSet<CartProduct> CartProducts { get; set; }
    public virtual DbSet<OfflineSale> OfflineSales { get; set; }
    public virtual DbSet<OfflineSaleItem> OfflineSaleItems { get; set; }
    public virtual DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }

    public override DbSet<AppRole> Roles { get; set; } = null!;
    public override DbSet<AppUserRole> UserRoles { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CartProduct>()
            .HasIndex(cp => new { cp.CartId, cp.ProductId })
            .IsUnique();

        builder.Entity<AppUserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<AppUserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }


    /* public virtual DbSet<CaseDetail> CaseDetails { get; set; }
     public virtual DbSet<CoolingSystemDetail> CoolingSystemDetails { get; set; }
     public virtual DbSet<MotherboardDetail> MotherboardDetails { get; set; }
     public virtual DbSet<PowerSupplyDetail> PowerSupplyDetails { get; set; }
     public virtual DbSet<ProcessorDetail> ProcessorDetails { get; set; }
     public virtual DbSet<RamDetail> RamDetails { get; set; }
     public virtual DbSet<StorageDetail> StorageDetails { get; set; }
     public virtual DbSet<VideoCardDetail> VideoCardDetails { get; set; }*/
}