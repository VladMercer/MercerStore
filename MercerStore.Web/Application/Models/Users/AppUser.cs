using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Models.Products;
using Microsoft.AspNetCore.Identity;

namespace MercerStore.Web.Application.Models.Users
{
    public class AppUser : IdentityUser
    {
        public string? UserImgUrl { get; set; }
        public string? Address { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? LastActivity { get; set; }
        public IEnumerable<Order>? Orders { get; set; }
        public IEnumerable<Review>? Reviews { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
