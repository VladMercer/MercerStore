using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api
{
    [ApiController]
    [Route("api/heartbeat")]
    public class HeartbeatController : ControllerBase
    {
       
        private readonly IUserActivityService _userActivityService;

        public HeartbeatController(IUserActivityService userActivityService)
        {
            _userActivityService = userActivityService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActivity()
        {
            await _userActivityService.UpdateUserActivity();
            return Ok();
        }
    }
}
