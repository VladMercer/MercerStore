using MercerStore.Web.Application.Dtos.OrderDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Application.ViewModels;
using MercerStore.Web.Application.ViewModels.Carts;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;
using MercerStore.Web.Infrastructure.Data.Enum.Order;

namespace MercerStore.Web.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userProfileRepository;

        public OrderService(IOrderRepository orderRepository, IUserRepository userProfileRepository)
        {
            _orderRepository = orderRepository;
            _userProfileRepository = userProfileRepository;
        }
        public async Task<PaginatedResultDto<OrderDto>> GetFilteredOrdersWithoutCache(OrderFilterRequest request)
        {
            var (orders, totalItems) = await _orderRepository.GetFilteredOrders(request);

            var orderDtos = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                PhoneNumber = order.PhoneNumber,
                Email = order.Email,
                Address = order.Address,
                UserId = order.UserId,
                GuestId = order.GuestId,
                TotalOrderPrice = order.TotalOrderPrice,
                Status = order.Status switch
                {
                    OrderStatuses.Pending => "На рассмотрении",
                    OrderStatuses.Cancelled => "Отмененный",
                    OrderStatuses.InProgress => "В процессе",
                    OrderStatuses.Failed => "Неудачный",
                    OrderStatuses.Completed => "Исполненный",
                    _ => "Неизвестный статус"
                },
                CreateDate = order.СreateDate,
                CompletedDate = order.CompletedDate,
            }).ToList();

            return new PaginatedResultDto<OrderDto>(orderDtos, totalItems, request.PageSize);
        }
        public async Task<Order> GetOrderById(int orderId)
        {
            return await _orderRepository.GetOrderById(orderId);
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserId(string userId)
        {
            return await _orderRepository.GetOrdersByUser(userId);
        }
        public async Task<Order> AddOrder(Order order)
        {
            return await _orderRepository.AddOrder(order);
        }
        public async Task UpdateOrderStatus(UpdateOrderStatusDto dto)
        {
            var order = await _orderRepository.GetOrderById(dto.OrderId);
            order.Status = dto.Status;
            await _orderRepository.UpdateOrder(order);
        }
        public async Task RemoveOrder(int orderId)
        {
            await _orderRepository.DeleteOrder(orderId);
        }
        public async Task RemoveOrderProduct(int orderId, int productId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            await _orderRepository.DeleteOrderProduct(order, productId);
        }
        public async Task<OrderViewModel> GetOrderViewModel(string userId, CartViewModel cartViewModel)
        {
            var User = await _userProfileRepository.GetUserByIdAsyncNoTracking(userId);

            return new OrderViewModel
            {
                Address = User?.Address,
                Email = User?.Email,
                CartViewModel = cartViewModel,
                PhoneNumber = User?.PhoneNumber
            };
        }

        public async Task<Order> CreateOrderFromCart(OrderViewModel orderViewModel, string currentUserId, IEnumerable<string> roles)
        {
            var isGuest = roles.Contains("Guest");

            var guestId = isGuest ? currentUserId : null;
            var userId = isGuest ? null : currentUserId;

            return await _orderRepository.CreateOrderFromCart(userId, guestId, orderViewModel);
        }
        public async Task<UpdateOrderViewModel> GetUpdateOrderViewModel(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            return new UpdateOrderViewModel
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
        }
        public async Task UpdateOder(UpdateOrderViewModel updateOrderViewModel)
        {
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

            await _orderRepository.UpdateOrderProductListTotalPrice(updateOrderViewModel.OrderProductListId, updatedTotalPrice);
        }
    }
}
