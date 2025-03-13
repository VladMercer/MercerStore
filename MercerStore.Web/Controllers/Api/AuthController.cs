using MercerStore.Web.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("generate-token")]
        public IActionResult GenerateToken()
        {
            var token = _authService.GenerateGuestToken();
            Response.Cookies.Append("OhCookies", token);

            return Ok(new { Token = token });
        }
    }
}
