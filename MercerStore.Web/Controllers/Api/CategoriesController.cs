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
    public async Task<IActionResult> GetFilteredProducts([FromQuery] CategoryFilterRequest request)
    {
        var result = await _mediator.Send(new GetFilteredProductsQuery(request));
        return Ok(result);
    }

    [HttpGet("price-range/{categoryId}")]
    public async Task<IActionResult> GetPriceRange(int categoryId)
    {
        var priceRange = await _mediator.Send(new GetPriceRangeQuery(categoryId));
        return Ok(priceRange);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _mediator.Send(new GetCategoriesQuery());
        return Ok(categories);
    }
}