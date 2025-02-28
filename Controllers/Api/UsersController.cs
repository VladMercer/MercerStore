using MercerStore.Data.Enum;
using MercerStore.Data.Enum.User;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly IUserRepository _userRepository;


        public UsersController(IUserIdentifierService userIdentifierService, IUserRepository userRepository)
        {
            _userIdentifierService = userIdentifierService;
            _userRepository = userRepository;
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
        [HttpGet("users/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetFilteredUsers(int pageNumber, int pageSize, UserSortOrder? sortOrder, TimePeriod? period, UserFilter? filter, string? query)
        {

            var (userDtos, totalUsers) = await _userRepository.GetFilteredUsers(pageNumber, pageSize, sortOrder, period, filter, query);

            var result = new
            {
                Users = userDtos,
                TotalItems = totalUsers,
                TotalPages = (int)Math.Ceiling((double)totalUsers / pageSize)
            };

            return Ok(result);
        }
    }
}
