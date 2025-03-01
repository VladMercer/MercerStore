using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [ApiController]
    [Route("api/heartbeat")]
    public class HeartbeatController : ControllerBase
    {
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IUserIdentifierService _userIdentifierService;

        public HeartbeatController(IUserActivityRepository userActivityRepository, IUserIdentifierService userIdentifierService)
        {
            _userActivityRepository = userActivityRepository;
            _userIdentifierService = userIdentifierService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActivity()
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            await _userActivityRepository.UpdateLastActivityAsync(userId, DateTime.UtcNow);
            return Ok();
        }
    }
}
