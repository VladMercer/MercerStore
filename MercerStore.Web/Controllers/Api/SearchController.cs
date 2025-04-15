using MediatR;
using MercerStore.Web.Application.Handlers.Search.Queries;
using MercerStore.Web.Application.Requests.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api
{
    [Authorize]
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchFilterRequest request)
        {
            var result = await _mediator.Send(new SearchProductQuery(request));
            return Ok(result);
        }
    }
}
