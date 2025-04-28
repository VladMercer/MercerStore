using Microsoft.AspNetCore.Identity;

namespace MercerStore.Web.Application.Models.Users;

public class AppRole : IdentityRole
{
    public virtual ICollection<AppUserRole> UserRoles { get; set; } = [];
}