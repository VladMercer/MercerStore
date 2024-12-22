using MercerStore.Data;
using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Repository
{
	public class OrderRepository : IOrderRepository
	{
		private readonly AppDbContext _context;

		public OrderRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task AddOrder(Order order)
		{
			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();
		}

		public async Task CreateOrderFromCart(string? userId, string? guestId, OrderViewModel orderViewModel)
		{
			var currentCart = _context.Carts
			 .Include(c => c.CartProducts)
			 .ThenInclude(cp => cp.Product)
			 .FirstOrDefault(c => c.AppUserId == userId || c.AppUserId == guestId);

			if (currentCart == null)
				throw new InvalidOperationException("Корзина не найдена");

			var orderProductList = new OrderProductList
			{
				UserId = userId ?? guestId,
				OrderProducts = currentCart.CartProducts.Select(cp => new OrderProduct
				{
					ProductId = cp.ProductId,
					Quantity = cp.Quantity
				}).ToList()
			};

			_context.OrderProductLists.Add(orderProductList);

            var order = new Order
            {
                PhoneNumber = orderViewModel.PhoneNumber,
                Email = orderViewModel.Email,
                Address = orderViewModel.Address,
                UserId = userId,
                GuestId = guestId,
                Date = DateTime.UtcNow,
                OrderProductList = orderProductList
            };

            _context.Orders.Add(order);

            
            currentCart.CartProducts.Clear();
            await _context.SaveChangesAsync();
        }

		public async Task DeleteOrder(int orderId)
		{
			var order = await _context.Orders.FindAsync(orderId);
			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Order>> GetAllOrders()
		{
			var orders = await _context.Orders
				.ToListAsync();
			return orders;
		}

		public async Task<Order> GetOrderById(int orderId)
		{
			var order = await _context.Orders.FindAsync(orderId);
			return order;
		}

		public async Task UpdateOrder(Order order)
		{
			_context.Update(order);
			await _context.SaveChangesAsync();
		}
	}
}
