using MercerStore.Web.Application.Dtos.OrderDto;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders/")]
        public async Task<IActionResult> GetFilteredOrders([FromQuery] OrderFilterRequest request)
        {
            var result = await _orderService.GetFilteredOrders(request);
            return Ok(result);
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            return Ok(order);
        }

        [HttpGet("orders/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(string userId)
        {
            var orders = await _orderService.GetOrderByUserId(userId);
            return Ok(orders);
        }

        [HttpPost("order")]
        [LogUserAction("User created an order", "order")]
        public async Task<IActionResult> AddOrder(Order order)
        {
            var newOrder = await _orderService.AddOrder(order);
            return Ok(newOrder);
        }

        [HttpPut("order/status")]
        [LogUserAction("User update order status", "order")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto dto)
        {
            await _orderService.UpdateOrderStatus(dto);
            return Ok(dto.OrderId);
        }

        [HttpDelete("order/{orderId}")]
        [LogUserAction("User delete order", "order")]
        public async Task<IActionResult> RemoveOrder(int orderId)
        {
            await _orderService.RemoveOrder(orderId);
            return Ok(orderId);
        }

        [HttpDelete("order/{orderId}/product/{productId}")]
        [LogUserAction("User removed product from order", "order")]
        public async Task<IActionResult> DeleteOrderProduct(int orderId, int productId)
        {
            await _orderService.RemoveOrderProduct(orderId, productId);
            return Ok(orderId);
        }
    }
}
