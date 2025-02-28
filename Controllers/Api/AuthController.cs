using MercerStore.Interfaces;
using MercerStore.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly double _expiresDays;
        private readonly IUserRepository _userProfileRepository;
        private readonly ILogService _logService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(IJwtProvider jwtProvider, IConfiguration configuration, IUserRepository userProfileRepository, ILogService logService, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _jwtProvider = jwtProvider;
            _expiresDays = double.Parse(configuration["JwtOptions:ExpiresDays"]);
            _userProfileRepository = userProfileRepository;
            _logService = logService;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost("generate-token")]
        public IActionResult GenerateToken()
        {

            var guestId = Guid.NewGuid().ToString();
            var roles = new List<string> { "Guest" };
            var token = _jwtProvider.GenerateJwtToken(guestId, roles, null, DateTime.UtcNow);

            Response.Cookies.Append("OhCookies", token);
            _logService.LogUserAction(
                roles: roles,
                userId: guestId,
                action: "Generate guest token",
                entityName: "Token",
                entityId: null,
                details: new
                {
                    token,
                }
            );

            return Ok(new
            {
                Token = token,
            });
        }
    }
}
