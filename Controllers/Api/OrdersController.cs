using MercerStore.Data.Enum;
using MercerStore.Data.Enum.Order;
using MercerStore.Dtos.InvoiceDto;
using MercerStore.Dtos.OrderDto;
using MercerStore.Dtos.ResultDto;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models.Orders;
using MercerStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Text.Json;

namespace MercerStore.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRequestContextService _requestContextService;
        private readonly IRedisCacheService _redisCacheService;
        public OrdersController(IOrderRepository orderRepository, IRequestContextService requestContextService, IRedisCacheService redisCacheService)
        {
            _orderRepository = orderRepository;
            _requestContextService = requestContextService;
            _redisCacheService = redisCacheService;
        }

        [HttpGet("orders/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetFilteredOrders(int pageNumber, int pageSize, OrdersSortOrder? sortOrder, TimePeriod? period, OrderStatuses? status, string? query)
        {
            bool isDefaultQuery =
                pageNumber == 1 &&
                pageSize == 30 &&
                !sortOrder.HasValue &&
                !status.HasValue &&
                query == "";

            string cacheKey = $"orders:page1";
            if (isDefaultQuery)
            {
                var cachedData = await _redisCacheService.GetCacheAsync<string>(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    return Ok(JsonSerializer.Deserialize<object>(cachedData));
                }
            }

            var (orderDtos, totalItems) = await _orderRepository.GetFilteredOrders(pageNumber, pageSize, sortOrder, period, status, query);

            var result = new PaginatedResultDto<OrderDto>(orderDtos, totalItems, pageSize);

            if (isDefaultQuery)
            {
                await _redisCacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            }

            return Ok(result);
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            return Ok(order);
        }

        [HttpGet("orders/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUser(userId);
            return Ok(orders);
        }

        [HttpPost("order")]
        [LogUserAction("User created an order", "order")]
        public async Task<IActionResult> AddOrder(Order order)
        {
            var newOrder = await _orderRepository.AddOrder(order);

            return Ok(newOrder);
        }

        [HttpPut("order/status")]
        [LogUserAction("User update order status", "order")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto dto)
        {
            var order = await _orderRepository.GetOrderById(dto.OrderId);
            order.Status = dto.Status;
            await _orderRepository.UpdateOrder(order);
            return Ok(dto.OrderId);
        }

        [HttpDelete("order/{orderId}")]
        [LogUserAction("User delete order", "order")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            await _orderRepository.DeleteOrder(orderId);
            return Ok(orderId);
        }

        [HttpDelete("order/{orderId}/product/{productId}")]
        [LogUserAction("User removed product from order", "order")]
        public async Task<IActionResult> DeleteOrderProduct(int orderId, int productId)
        {
            var order = await _orderRepository.GetOrderById(orderId);

            await _orderRepository.DeleteOrderProduct(order, productId);
            var logDetails = new
            {
                productId
            };

            _requestContextService.SetLogDetails(logDetails);

            return Ok(orderId);
        }
    }
}
