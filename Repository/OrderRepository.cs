using MercerStore.Data;
using MercerStore.Data.Enum;
using MercerStore.Data.Enum.Order;
using MercerStore.Dtos.OrderDto;
using MercerStore.Interfaces;
using MercerStore.Models.Orders;
using MercerStore.ViewModels;
using Microsoft.CodeAnalysis;
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

        public async Task<Order> AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> CreateOrderFromCart(string? userId, string? guestId, OrderViewModel orderViewModel)
        {
            var currentCart = _context.Carts
             .Include(c => c.CartProducts)
             .ThenInclude(cp => cp.Product)
             .ThenInclude(cp => cp.ProductPricing)
             .FirstOrDefault(c => c.AppUserId == userId || c.AppUserId == guestId);

            if (currentCart == null)
                throw new InvalidOperationException("Корзина не найдена");

            var orderProductList = new OrderProductList
            {
                UserId = userId ?? guestId,

                OrderProductSnapshots = currentCart.CartProducts.Select(cp => new OrderProductSnapshot
                {
                    ProductId = cp.ProductId,
                    ProductName = cp.Product.Name,
                    Quantity = cp.Quantity,
                    PriceAtOrder = cp.Quantity * (cp.Product.ProductPricing.DiscountedPrice ??
                    cp.Product.ProductPricing.OriginalPrice),
                    ProductImageUrl = cp.Product.MainImageUrl
                }).ToList(),

                TotalPrice = currentCart.CartProducts.Sum(ci => (ci.Product.ProductPricing.DiscountedPrice
                ?? ci.Product.ProductPricing.OriginalPrice) * ci.Quantity)
            };

            _context.OrderProductLists.Add(orderProductList);

            var order = new Order
            {
                PhoneNumber = orderViewModel.PhoneNumber,
                Email = orderViewModel.Email,
                Address = orderViewModel.Address,
                UserId = userId,
                GuestId = guestId,
                СreateDate = DateTime.UtcNow,
                OrderProductList = orderProductList,
                TotalOrderPrice = orderProductList.TotalPrice,
                Status = OrderStatuses.Pending
            };

            _context.Orders.Add(order);

            currentCart.CartProducts.Clear();
            await _context.SaveChangesAsync();
            return order;
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

        public async Task<(IEnumerable<OrderDto>, int totalItems)> GetFilteredOrders(
            int pageNumber,
            int pageSize,
            OrdersSortOrder? sortOrder,
            TimePeriod? timePeriod,
            OrderStatuses? orderStatuses,

            string query)
        {
            var currentDay = DateTime.UtcNow;
            var ordersQuery = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                ordersQuery = ordersQuery.Where(u =>
                    EF.Functions.ILike(u.Email, $"%{query}%") ||
                    EF.Functions.ILike(u.Address, $"%{query}%") ||
                    EF.Functions.ILike(u.PhoneNumber, $"%{query}%")
                );
            }


            if (timePeriod.HasValue && timePeriod != TimePeriod.All)
            {
                var filterDate = timePeriod switch
                {
                    TimePeriod.Day => currentDay.AddDays(-1),
                    TimePeriod.Week => currentDay.AddDays(-7),
                    TimePeriod.Month => currentDay.AddMonths(-1),
                    TimePeriod.Quarter => currentDay.AddMonths(-3),
                    TimePeriod.Year => currentDay.AddYears(-1),
                    _ => currentDay,
                };

                ordersQuery = ordersQuery.Where(o => o.СreateDate >= filterDate);
            }

            if (orderStatuses.HasValue && orderStatuses != OrderStatuses.All)
            {
                ordersQuery = orderStatuses switch
                {
                    OrderStatuses.Pending => ordersQuery.Where(o => o.Status == OrderStatuses.Pending),
                    OrderStatuses.InProgress => ordersQuery.Where(o => o.Status == OrderStatuses.InProgress),
                    OrderStatuses.Completed => ordersQuery.Where(o => o.Status == OrderStatuses.Completed),
                    OrderStatuses.Cancelled => ordersQuery.Where(o => o.Status == OrderStatuses.Cancelled),
                    OrderStatuses.Failed => ordersQuery.Where(o => o.Status == OrderStatuses.Failed),
                    _ => ordersQuery,
                };
            }

            ordersQuery = sortOrder switch
            {
                OrdersSortOrder.DateTimeAsc => ordersQuery.OrderBy(p => p.СreateDate),
                OrdersSortOrder.DateTimeDesc => ordersQuery.OrderByDescending(p => p.СreateDate),
                OrdersSortOrder.StatusAsc => ordersQuery.OrderBy(p => p.Status),
                OrdersSortOrder.StatusDesc => ordersQuery.OrderByDescending(p => p.Status),
                OrdersSortOrder.TotalPriceAsc => ordersQuery.OrderBy(p => p.TotalOrderPrice),
                OrdersSortOrder.TotalPriceDesc => ordersQuery.OrderByDescending(p => p.TotalOrderPrice),
                _ => ordersQuery.OrderBy(p => p.СreateDate),
            };

            var totalItems = await ordersQuery.CountAsync();

            var orders = await ordersQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

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

            return (orderDtos, totalItems);
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProductList)
                .ThenInclude(o => o.OrderProductSnapshots)
                .FirstAsync(o => o.Id == orderId);
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUser(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId || o.GuestId == userId).ToListAsync();
        }

        public async Task<List<OrderProductSnapshot>> GetOrderItemsById(int orderId)
        {
            return await _context.OrderProductSnapshots.Where(o => o.Id == orderId).ToListAsync();
        }
        public async Task UpdateOrder(Order order)
        {
            _context.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateOrderItems(List<OrderProductSnapshot> orderItems)
        {
            foreach (var item in orderItems)
            {
                _context.Attach(item);
                _context.Entry(item).Property(x => x.Quantity).IsModified = true;
            }

            await _context.SaveChangesAsync();
        }
        public async Task UpdateOrderProductListTotalPrice(int orderProductListId, decimal newTotalPrice)
        {
            var orderProductList = await _context.OrderProductLists.FindAsync(orderProductListId);
            if (orderProductList != null)
            {
                orderProductList.TotalPrice = newTotalPrice;
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteOrderProduct(Order order, int productId)
        {

            var snapshotToRemove = order.OrderProductList.OrderProductSnapshots
            .FirstOrDefault(s => s.ProductId == productId);

            order.OrderProductList.OrderProductSnapshots.Remove(snapshotToRemove);

            await _context.SaveChangesAsync();
        }

        public async Task<object> GetSalesMetric()
        {
            var now = DateTime.UtcNow;
            var startOfWeek = now.AddDays(-(int)now.DayOfWeek);
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfQuarter = now.AddMonths(-(int)startOfMonth.Month);
            var startOfYear = new DateTime(now.Year, 1, 1);

            var sales = await _context.Orders
                .AsNoTracking()
                .Where(o => o.Status == OrderStatuses.Completed)
                .GroupBy(o => o.CompletedDate.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.TotalOrderPrice),
                    Orders = g.Count()
                })
                .ToListAsync();

            var salesMetric = new
            {
                Daily = sales.Where(x => x.Date == now.Date).Sum(x => x.Revenue),
                Weekly = sales.Where(x => x.Date >= startOfWeek).Sum(x => x.Revenue),
                Monthly = sales.Where(x => x.Date >= startOfMonth).Sum(x => x.Revenue),
                Yearly = sales.Where(x => x.Date >= startOfYear).Sum(x => x.Revenue),
                TotalOrders = new
                {
                    Daily = sales.Where(x => x.Date == now.Date).Sum(x => x.Orders),
                    Weekly = sales.Where(x => x.Date >= startOfWeek).Sum(x => x.Orders),
                    Monthly = sales.Where(x => x.Date >= startOfMonth).Sum(x => x.Orders),
                    Yearly = sales.Where(x => x.Date >= startOfYear).Sum(x => x.Orders)
                },
                AverageOrderValue = _context.Orders
                    .AsNoTracking()
                    .Average(o => o.TotalOrderPrice),
                TopProducts = await _context.OrderProductSnapshots
                    .AsNoTracking()
                    .GroupBy(p => p.ProductName)
                    .Select(g => new
                    {
                        Name = g.Key,
                        Sales = g.Sum(x => x.Quantity)
                    })
                    .OrderByDescending(x => x.Sales)
                    .Take(5)
                    .ToListAsync()
            };

            return salesMetric;
        }
        public async Task<decimal> GetRevenue()
        {
            return await _context.Orders.AsNoTracking().SumAsync(o => o.TotalOrderPrice);
        }
        public async Task<int> GetOrdersCount()
        {
            return await _context.Orders.AsNoTracking().CountAsync();
        }
    }
}
