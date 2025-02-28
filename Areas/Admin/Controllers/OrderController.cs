using MercerStore.Areas.Admin.ViewModels;
using MercerStore.Data.Enum.Order;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models.Orders;
using MercerStore.Models.Products;
using MercerStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Validations;
using System.Collections.Immutable;

namespace MercerStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRequestContextService _requestContextService;
      

        public OrderController(IOrderRepository orderRepository, IRequestContextService requestContextService, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _requestContextService = requestContextService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[area]/[controller]/update/{orderId}")]
        public async Task<IActionResult> Details(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            var orderUpdateViewModel = new UpdateOrderViewModel
            {
                Id = orderId,
                GuestId = order.GuestId,
                UserId = order.UserId,
                Address = order.Address,
                CompletedDate = order.CompletedDate,
                CreateDate = order.СreateDate,
                PhoneNumber = order.PhoneNumber,
                OrderItems = order.OrderProductList.OrderProductSnapshots.Select(o => new OrderProductSnapshotViewModel
                {
                    Id = o.Id,
                    PriceAtOrder = o.PriceAtOrder,
                    ProductId = o.ProductId,
                    ProductImageUrl = o.ProductImageUrl,
                    ProductName = o.ProductName,
                    Quantity = o.Quantity
                }).ToList(),
                Email = order.Email,
                Status = order.Status,
                TotalOrderPrice = order.TotalOrderPrice,
                OrderProductListId = order.OrderProductListId

            };
            return View(orderUpdateViewModel);
        }

        [HttpPost("[area]/[controller]/update/")]
        [LogUserAction("Manager update order", "order")]
        public async Task<IActionResult> Details(UpdateOrderViewModel updateOrderViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateOrderViewModel);
            }

            var updatedTotalPrice = updateOrderViewModel.OrderItems.Sum(o => o.Quantity * o.PriceAtOrder);

            if (updateOrderViewModel.Status == OrderStatuses.Completed)
            {
                updateOrderViewModel.CompletedDate = DateTime.UtcNow;
            }

            var updateOrder = new Order
            {
                Id = updateOrderViewModel.Id,
                Address = updateOrderViewModel.Address,
                CompletedDate = updateOrderViewModel.CompletedDate,
                СreateDate = updateOrderViewModel.CreateDate,
                Email = updateOrderViewModel.Email,
                GuestId = updateOrderViewModel.GuestId,
                UserId = updateOrderViewModel.UserId,
                TotalOrderPrice = updatedTotalPrice,

                PhoneNumber = updateOrderViewModel.PhoneNumber,
                Status = updateOrderViewModel.Status,
                OrderProductListId = updateOrderViewModel.OrderProductListId

            };
            await _orderRepository.UpdateOrder(updateOrder);

            var updateOrderItems = updateOrderViewModel.OrderItems.Select(o => new OrderProductSnapshot
            {
                Id = o.Id,
                Quantity = o.Quantity,
            }).ToList();

            await _orderRepository.UpdateOrderItems(updateOrderItems);

            var logDetails = new
            {
                updateOrderViewModel.Status,
            };


            await _orderRepository.UpdateOrderProductListTotalPrice(updateOrderViewModel.OrderProductListId, updatedTotalPrice);

            _requestContextService.SetLogDetails(logDetails);

            return RedirectToAction("Index", new { id = updateOrderViewModel.Id });
        }
    }
}
