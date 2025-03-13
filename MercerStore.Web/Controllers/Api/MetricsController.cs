using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/metrics")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
     
        private readonly IMetricService _metricService;

        public MetricsController(IMetricService metricService)
        {
            _metricService = metricService;
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            var metrics = await _metricService.GetMetrics();
            return Ok(metrics);
        }
    }
}
