using MercerStore.Web.Application.Dtos.Order;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Application.ViewModels;
using MercerStore.Web.Application.ViewModels.Carts;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IOrderService
{
    Task<PaginatedResultDto<OrderDto>> GetFilteredOrdersWithoutCache(OrderFilterRequest request, CancellationToken ct);
    Task<Order> GetOrderById(int orderId, CancellationToken ct);
    Task<IEnumerable<Order>> GetOrdersByUserId(string userId, CancellationToken ct);
    Task<Order> AddOrder(Order order, CancellationToken ct);
    Task UpdateOrderStatus(UpdateOrderStatusDto dto, CancellationToken ct);
    Task RemoveOrder(int orderId, CancellationToken ct);
    Task RemoveOrderProduct(int orderId, int productId, CancellationToken ct);
    Task<OrderViewModel> GetOrderViewModel(string userId, CartViewModel cartViewModel, CancellationToken ct);

    Task<Order> CreateOrderFromCart(OrderViewModel orderViewModel, string currentUserId, IEnumerable<string> roles,
        CancellationToken ct);

    Task<UpdateOrderViewModel> GetUpdateOrderViewModel(int orderId, CancellationToken ct);
    Task UpdateOder(UpdateOrderViewModel updateOrderViewModel, CancellationToken ct);
}
