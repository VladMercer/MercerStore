using MercerStore.Models;
using MercerStore.ViewModels;

namespace MercerStore.Interfaces
{
	public interface IOrderRepository
	{
		Task<IEnumerable<Order>> GetAllOrders();
		Task<Order> GetOrderById(int orderId);
        Task CreateOrderFromCart(string userId, string? guestId, OrderViewModel model);
        Task AddOrder(Order order);
		Task UpdateOrder(Order order);
		Task DeleteOrder(int orderId);

	}
}
