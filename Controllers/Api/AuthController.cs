using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly double _expiresDays;
        public AuthController(IJwtProvider jwtProvider, IConfiguration configuration)
        {
            _jwtProvider = jwtProvider;
            _expiresDays = double.Parse(configuration["JwtOptions:ExpiresDays"]);
        }
        [HttpPost("generate-token")]
        public IActionResult GenerateToken()
        {
            var guestId = Guid.NewGuid().ToString();
            var roles = new List<string> { "Guest" };
            var token = _jwtProvider.GenerateJwtToken(guestId, roles);

            Response.Cookies.Append("OhCookies", token);

            return Ok(new
            {
                Token = token,
            });
        }
    }
}
