using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Account;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MercerStore.Web.Infrastructure.Services;

public class JwtProvider : IJwtProvider
{
    private readonly IOptions<JwtOptions> _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public async Task<string> GenerateJwtToken(JwtTokenRequest request)
    {
        const string defaultProfilePictureUrl = "https://localhost:7208/img/default/default_user_image.jpg";
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, request.UserId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(request.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var resolvedProfilePictureUrl = request.ProfilePictureUrl ?? defaultProfilePictureUrl;

        claims.Add(new Claim("profile_picture", resolvedProfilePictureUrl));
        claims.Add(new Claim("creation_date", request.CreationDate.ToString("o")));
        claims.Add(new Claim("time_zone", request.TimeZone!));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_jwtOptions.Value.ExpiresDays),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
