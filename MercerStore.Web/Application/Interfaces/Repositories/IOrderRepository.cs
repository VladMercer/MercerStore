using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Application.ViewModels;

namespace MercerStore.Web.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<IList<Order>> GetAllOrders(CancellationToken ct);
    Task<Order> GetOrderById(int orderId, CancellationToken ct);

    Task<Order> CreateOrderFromCart(string? userId, string? guestId, OrderViewModel orderViewModel,
        CancellationToken ct);

    Task<Order> AddOrder(Order order, CancellationToken ct);
    Task UpdateOrder(Order order, CancellationToken ct);
    Task DeleteOrder(int orderId, CancellationToken ct);
    Task<IList<Order>> GetOrdersByUser(string userId, CancellationToken ct);

    Task<(IList<Order>, int totalItems)> GetFilteredOrders(OrderFilterRequest request,
        CancellationToken ct);

    Task<IList<OrderProductSnapshot>> GetOrderItemsById(int orderId, CancellationToken ct);

    Task UpdateOrderProductListTotalPrice(int orderProductListId, decimal newTotalPrice,
        CancellationToken ct);

    Task UpdateOrderItems(IEnumerable<OrderProductSnapshot> orderItems, CancellationToken ct);
    Task DeleteOrderProduct(Order order, int productId, CancellationToken ct);
    Task<SalesMetricDto> GetSalesMetric(CancellationToken ct);
    Task<decimal> GetRevenue(CancellationToken ct);
    Task<int> GetOrdersCount(CancellationToken ct);
}
