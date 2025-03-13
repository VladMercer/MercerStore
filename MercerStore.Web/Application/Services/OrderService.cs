using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.OrderDto;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Infrastructure.Data.Enum.Order;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.ViewModels;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;

namespace MercerStore.Web.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRequestContextService _requestContextService;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly ICartProductRepository _cartProductRepository;
        private readonly IUserRepository _userProfileRepository;
        private readonly ICartService _cartService;

        public OrderService(
            IOrderRepository orderRepository,
            IRequestContextService requestContextService,
            IRedisCacheService redisCacheService,
            IUserIdentifierService userIdentifierService,
            ICartProductRepository cartProductRepository,
            IUserRepository userProfileRepository,
            ICartService cartService)
        {
            _orderRepository = orderRepository;
            _requestContextService = requestContextService;
            _redisCacheService = redisCacheService;
            _userIdentifierService = userIdentifierService;
            _cartProductRepository = cartProductRepository;
            _userProfileRepository = userProfileRepository;
            _cartService = cartService;
        }
        public async Task<PaginatedResultDto<OrderDto>> GetFilteredOrders(OrderFilterRequest request)
        {

            bool isDefaultQuery =
            request.PageNumber == 1 &&
            request.PageSize == 30 &&
            !request.SortOrder.HasValue &&
            !request.Status.HasValue &&
            request.Query == "";

            string cacheKey = $"orders:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => FetchFilteredOrders(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
        public async Task<Order> GetOrderById(int orderId)
        {
            return await _orderRepository.GetOrderById(orderId);
        }
        public async Task<IEnumerable<Order>> GetOrderByUserId(string userId)
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

            var logDetails = new
            {
                productId
            };

            _requestContextService.SetLogDetails(logDetails);
        }
        public async Task<OrderViewModel> GetOrderViewModel()
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var User = await _userProfileRepository.GetUserByIdAsyncNoTracking(userId);
            var cartViewModel = await _cartService.GetCartViewModel();

            return new OrderViewModel
            {
                Address = User?.Address,
                Email = User?.Email,
                CartViewModel = cartViewModel,
                PhoneNumber = User?.PhoneNumber
            };
        }
        private async Task<PaginatedResultDto<OrderDto>> FetchFilteredOrders(OrderFilterRequest request)
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

            var result = new PaginatedResultDto<OrderDto>(orderDtos, totalItems, request.PageSize);
            return result;
        }
        public async Task<int> CreateOrderFromCart(OrderViewModel orderViewModel)
        {
            var currentUserId = _userIdentifierService.GetCurrentIdentifier();
            var roles = _userIdentifierService.GetCurrentUserRoles();
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

            var logDetails = new
            {
                updateOrderViewModel.Status,
            };

            await _orderRepository.UpdateOrderProductListTotalPrice(updateOrderViewModel.OrderProductListId, updatedTotalPrice);

            _requestContextService.SetLogDetails(logDetails);
        }
    }
}
