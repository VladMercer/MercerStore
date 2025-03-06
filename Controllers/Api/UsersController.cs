using MercerStore.Data.Enum;
using MercerStore.Data.Enum.User;
using MercerStore.Dtos.ResultDto;
using MercerStore.Dtos.SupplierDto;
using MercerStore.Dtos.UserDto;
using MercerStore.Interfaces;
using MercerStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MercerStore.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly IUserRepository _userRepository;
        private readonly IRedisCacheService _redisCacheService;


        public UsersController(IUserIdentifierService userIdentifierService, IUserRepository userRepository, IRedisCacheService redisCacheService)
        {
            _userIdentifierService = userIdentifierService;
            _userRepository = userRepository;
            _redisCacheService = redisCacheService;
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
            bool isDefaultQuery =
                pageNumber == 1 &&
                pageSize == 30 &&
                !sortOrder.HasValue &&
                !period.HasValue && 
                !filter.HasValue &&
                string.IsNullOrEmpty(query);

            string cacheKey = $"users:page1";
            if (isDefaultQuery)
            {
                var cachedData = await _redisCacheService.GetCacheAsync<string>(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    return Ok(JsonSerializer.Deserialize<object>(cachedData));
                }
            }

            var (userDtos, totalUsers) = await _userRepository.GetFilteredUsers(pageNumber, pageSize, sortOrder, period, filter, query);

            var result = new PaginatedResultDto<UserDto>(userDtos, totalUsers, pageSize);

            if (isDefaultQuery)
            {
                await _redisCacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            }

            return Ok(result);
        }
    }
}
