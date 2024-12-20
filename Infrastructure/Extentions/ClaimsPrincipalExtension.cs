using System.Security.Claims;

namespace MercerStore.Infrastructure.Extentions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
		public static string GetUserRoles(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Role)?.Value;
		}
	}
}