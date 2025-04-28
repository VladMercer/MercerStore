using MediatR;
using MercerStore.Web.Application.Handlers.Products.Queries;
using MercerStore.Web.Application.Requests.Products;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api;

[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetAdminFilteredProducts([FromQuery] ProductFilterRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAdminFilteredProductsQuery(request), ct);
        return Ok(result);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProduct(int productId, CancellationToken ct)
    {
        var product = await _mediator.Send(new GetProductQuery(productId), ct);
        return Ok(product);
    }

    [HttpGet("product/sku")]
    public async Task<IActionResult> GetProductSku(int productId, CancellationToken ct)
    {
        var Sku = await _mediator.Send(new GetProductQuery(productId), ct);
        return Ok(Sku);
    }
}