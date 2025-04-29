using MediatR;
using MercerStore.Web.Application.Handlers.Categories.Queries;
using MercerStore.Web.Application.Requests.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api;

[Authorize]
[Route("api/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetFilteredProducts([FromQuery] CategoryFilterRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetFilteredProductsQuery(request), ct);
        return Ok(result);
    }

    [HttpGet("price-range/{categoryId}")]
    public async Task<IActionResult> GetPriceRange(int categoryId, CancellationToken ct)
    {
        var priceRange = await _mediator.Send(new GetPriceRangeQuery(categoryId), ct);
        return Ok(priceRange);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories(CancellationToken ct)
    {
        var categories = await _mediator.Send(new GetCategoriesQuery(), ct);
        return Ok(categories);
    }
}