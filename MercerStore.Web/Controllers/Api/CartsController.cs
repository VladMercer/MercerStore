using MediatR;
using MercerStore.Web.Application.Handlers.Carts.Commands;
using MercerStore.Web.Application.Handlers.Carts.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api;

[Authorize]
[Route("api/cart")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(CancellationToken ct)
    {
        var cartViewModel = await _mediator.Send(new GetCartViewModelQuery(), ct);
        return Ok(cartViewModel);
    }

    [HttpGet("itemCount")]
    public async Task<IActionResult> GetCartItemCount(CancellationToken ct)
    {
        var itemCount = await _mediator.Send(new GetCartItemCountQuery(), ct);
        return Ok(itemCount);
    }

    [HttpPost("product/{productId}")]
    public async Task<IActionResult> AddToCart(int productId, CancellationToken ct)
    {
        await _mediator.Send(new AddToCartCommand(productId));
        return Ok();
    }

    [HttpDelete("product/{productId}")]
    public async Task<IActionResult> RemoveFromCart(int productId, CancellationToken ct)
    {
        await _mediator.Send(new RemoveFromCartCommand(productId));
        return Ok();
    }
}