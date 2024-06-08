using Microsoft.AspNetCore.Identity;

namespace MercerStore.Models
{
    public class AppUser : IdentityUser
    {
        public string? UserImgUrl { get; set; }
        public string? Adress { get; set; }
    }
}
