using MercerStore.Web.Application.Dtos.MetricDto;
using MercerStore.Web.Application.Dtos.OrderDto;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Application.ViewModels;
using MercerStore.Web.Infrastructure.Data.Enum;
using MercerStore.Web.Infrastructure.Data.Enum.Order;

namespace MercerStore.Web.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int orderId);
        Task<int> CreateOrderFromCart(string? userId, string? guestId, OrderViewModel orderViewModel);
        Task<Order> AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUser(string userId);
        Task<(IEnumerable<Order>, int totalItems)> GetFilteredOrders(OrderFilterRequest request);
        Task<List<OrderProductSnapshot>> GetOrderItemsById(int orderId);
        Task UpdateOrderProductListTotalPrice(int orderProductListId, decimal newTotalPrice);
        Task UpdateOrderItems(List<OrderProductSnapshot> orderItems);
        Task DeleteOrderProduct(Order order, int productId);
        Task<SalesMetricDto> GetSalesMetric();
        Task<decimal> GetRevenue();
        Task<int> GetOrdersCount();
    }
}
