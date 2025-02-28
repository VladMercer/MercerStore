using MercerStore.Data.Enum;
using MercerStore.Data.Enum.Order;
using MercerStore.Dtos.OrderDto;
using MercerStore.Models.Orders;
using MercerStore.ViewModels;

namespace MercerStore.Interfaces
{
    public interface IOrderRepository
	{
		Task<IEnumerable<Order>> GetAllOrders();
		Task<Order> GetOrderById(int orderId);
        Task<Order> CreateOrderFromCart(string userId, string? guestId, OrderViewModel model);
        Task<Order> AddOrder(Order order);
		Task UpdateOrder(Order order);
		Task DeleteOrder(int orderId);
		Task<IEnumerable<Order>> GetOrdersByUser(string userId);
        Task<(IEnumerable<OrderDto>, int totalItems)> GetFilteredOrders(
          int pageNumber,
          int pageSize,
          OrdersSortOrder? sortOrder,
          TimePeriod? timePeriod,
          OrderStatuses? orderStatuses,
          string query);
        Task<List<OrderProductSnapshot>> GetOrderItemsById(int orderId);
        Task UpdateOrderProductListTotalPrice(int orderProductListId, decimal newTotalPrice);
        Task UpdateOrderItems(List<OrderProductSnapshot> orderItems);
        Task DeleteOrderProduct(Order order, int productId);
        Task<object> GetSalesMetric();


    }
}
