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
    public async Task<IActionResult> GetAdminFilteredProducts([FromQuery] ProductFilterRequest request)
    {
        var result = await _mediator.Send(new GetAdminFilteredProductsQuery(request));
        return Ok(result);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var product = await _mediator.Send(new GetProductQuery(productId));
        return Ok(product);
    }

    [HttpGet("product/sku")]
    public async Task<IActionResult> GetProductSku(int productId)
    {
        var Sku = await _mediator.Send(new GetProductQuery(productId));
        return Ok(Sku);
    }
}