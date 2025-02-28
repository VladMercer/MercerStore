using Microsoft.AspNetCore.Identity;

namespace MercerStore.Models.Users
{
    public class AppRole : IdentityRole
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
