using MediatR;
using MercerStore.Web.Application.Dtos.Order;
using MercerStore.Web.Application.Handlers.Orders.Commands;
using MercerStore.Web.Application.Handlers.Orders.Queries;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api;

[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("orders/")]
    public async Task<IActionResult> GetFilteredOrders([FromQuery] OrderFilterRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetFilteredOrdersQuery(request), ct);
        return Ok(result);
    }

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetOrderById(int orderId, CancellationToken ct)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(orderId), ct);
        return Ok(order);
    }

    [HttpGet("orders/{userId}")]
    public async Task<IActionResult> GetOrdersByUser(string userId, CancellationToken ct)
    {
        var orders = await _mediator.Send(new GetOrdersByUserQuery(userId), ct);
        return Ok(orders);
    }

    [HttpPost("order")]
    public async Task<IActionResult> AddOrder(Order order, CancellationToken ct)
    {
        await _mediator.Send(new AddOrderCommand(order), ct);
        return Ok();
    }

    [HttpPut("order/status")]
    public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto dto, CancellationToken ct)
    {
        await _mediator.Send(new UpdateOrderStatusCommand(dto), ct);
        return Ok();
    }

    [HttpDelete("order/{orderId}")]
    public async Task<IActionResult> RemoveOrder(int orderId, CancellationToken ct)
    {
        await _mediator.Send(new RemoveOrderCommand(orderId), ct);
        return Ok();
    }

    [HttpDelete("order/{orderId}/product/{productId}")]
    public async Task<IActionResult> DeleteOrderProduct(int orderId, int productId, CancellationToken ct)
    {
        await _mediator.Send(new RemoveOrderProductCommand(orderId, productId), ct);
        return Ok();
    }
}