using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
	[Route("api/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserIdentifierService _userIdentifierService;

		public UsersController(IUserIdentifierService userIdentifierService)
		{
			_userIdentifierService = userIdentifierService;
		}

		[HttpGet("userId")]
		public IActionResult GetCurrentUserId()
		{
			var userId = _userIdentifierService.GetCurrentIdentifier();
			if (userId == null)
			{
				return BadRequest();
			}
			return Ok(userId);
		}

		[HttpGet("roles")]
		public IActionResult GetCurrentUserRoles()
		{
			var roles = _userIdentifierService.GetCurrentUserRoles();
			if (roles == null)
			{
				return BadRequest();
			}
			return Ok(roles);
		}
	}
}
