using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserIdentifierService _userIdentifierService;

        public UsersController(IUserService userService, IUserIdentifierService userIdentifierService)
        {
            _userService = userService;
            _userIdentifierService = userIdentifierService;
        }

        [HttpGet("userId")]
        public IActionResult GetCurrentUserId()
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            return Ok(userId);
        }

        [HttpGet("roles")]
        public IActionResult GetCurrentUserRoles()
        {
            var roles = _userIdentifierService.GetCurrentUserRoles();
            return Ok(roles);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetFilteredUsers([FromQuery] UserFilterRequest request)
        {
            var result = await _userService.GetFilteredUsers(request);
            return Ok(result);
        }
    }
}
