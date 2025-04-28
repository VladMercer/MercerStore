using System.Security.Claims;

namespace MercerStore.Web.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public static IEnumerable<string> GetUserRoles(this ClaimsPrincipal user)
    {
        return user.FindAll(ClaimTypes.Role).Select(role => role.Value).ToList();
    }

    public static string GetProfilePictureUrl(this ClaimsPrincipal user)
    {
        return user?.FindFirst("profile_picture")?.Value;
    }

    public static string? GetUserTimeZone(this ClaimsPrincipal user)
    {
        return user?.FindFirst("time_zone")?.Value;
    }
}
