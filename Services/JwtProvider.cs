using MercerStore.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MercerStore.Services
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

        public string GenerateJwtToken(string userId, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userId),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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

