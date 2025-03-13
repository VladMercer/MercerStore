using MercerStore.Web.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MercerStore.Web.Infrastructure.Services
{
    public class JwtProvider : IJwtProvider
    {
        private readonly string _secretKey;
        private readonly double _expiresDays;
        public JwtProvider(IConfiguration configuration)
        {
            _secretKey = configuration["JwtOptions:SecretKey"];
            _expiresDays = double.Parse(configuration["JwtOptions:ExpiresDays"]);

        }

        public string GenerateJwtToken(string userId, IEnumerable<string> roles, string? profilePictureUrl, DateTime creationDate)
        {
            var defaultProfilePictureUrl = "https://localhost:7208/img/default/default_user_image.jpg";
            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userId),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var resolvedProfilePictureUrl = profilePictureUrl ?? defaultProfilePictureUrl;

            claims.Add(new Claim("profile_picture", resolvedProfilePictureUrl));
            claims.Add(new Claim("creation_date", creationDate.ToString("o")));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_expiresDays),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

