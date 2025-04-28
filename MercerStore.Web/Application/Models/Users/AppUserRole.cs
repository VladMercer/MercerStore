using Microsoft.AspNetCore.Identity;

namespace MercerStore.Web.Application.Models.Users;

public class AppUserRole : IdentityUserRole<string>
{
    public virtual AppUser User { get; set; } = null!;
    public virtual AppRole Role { get; set; } = null!;
}