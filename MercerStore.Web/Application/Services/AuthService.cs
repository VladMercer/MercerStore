using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Account;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Services;

public class AuthService : IAuthService
{
    private readonly IJwtProvider _jwtProvider;

    public AuthService(IJwtProvider jwtProvider)
    {
        _jwtProvider = jwtProvider;
    }

    public async Task<(string, string)> GenerateGuestToken(GenerateTokenRequest request)
    {
        var guestId = Guid.NewGuid().ToString();
        var roles = new List<string> { RoleNames.Guest };

        var token = await _jwtProvider.GenerateJwtToken(
            new JwtTokenRequest(guestId, roles, null, DateTime.UtcNow, request.TimeZone));

        return (token, guestId);
    }
}