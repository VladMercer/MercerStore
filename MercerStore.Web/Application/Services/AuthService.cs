using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly ILogService _logService;
        private readonly double _expiresDays;

        public AuthService(IJwtProvider jwtProvider, IConfiguration configuration, ILogService logService)
        {
            _jwtProvider = jwtProvider;
            _logService = logService;
            _expiresDays = double.Parse(configuration["JwtOptions:ExpiresDays"]);
        }

        public string GenerateGuestToken()
        {
            var guestId = Guid.NewGuid().ToString();
            var roles = new List<string> { "Guest" };
            var token = _jwtProvider.GenerateJwtToken(guestId, roles, null, DateTime.UtcNow);

            _logService.LogUserAction(
                roles: roles,
                userId: guestId,
                action: "Generate guest token",
                entityName: "Token",
                entityId: null,
                details: new { token }
            );

            return token;
        }
    }
    }
