using MercerStore.Web.Application.Interfaces.Services;
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

        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchFilterRequest request)
        {
            var result = await _searchService.SearchProduct(request);
            return Ok(result);
        }
    }
}
