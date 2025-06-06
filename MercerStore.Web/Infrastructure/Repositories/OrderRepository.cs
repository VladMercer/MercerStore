﻿using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Application.ViewModels;
using MercerStore.Web.Infrastructure.Data;
using MercerStore.Web.Infrastructure.Data.Enum;
using MercerStore.Web.Infrastructure.Data.Enum.Order;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> AddOrder(Order order, CancellationToken ct)
    {
        await _context.Orders.AddAsync(order, ct);
        await _context.SaveChangesAsync(ct);
        return order;
    }

    public async Task<Order> CreateOrderFromCart(string? userId, string? guestId, OrderViewModel orderViewModel,
        CancellationToken ct)
    {
        var currentCart = await _context.Carts
            .Include(c => c.CartProducts)
            .ThenInclude(cp => cp.Product)
            .ThenInclude(cp => cp.ProductPricing)
            .FirstOrDefaultAsync(c => c.AppUserId == userId || c.AppUserId == guestId, ct);

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
        await _context.SaveChangesAsync(ct);
        return order;
    }

    public async Task DeleteOrder(int orderId, CancellationToken ct)
    {
        var order = await _context.Orders.FindAsync(orderId, ct);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IList<Order>> GetAllOrders(CancellationToken ct)
    {
        var orders = await _context.Orders
            .ToListAsync(ct);
        return orders;
    }

    public async Task<(IList<Order>, int totalItems)> GetFilteredOrders(OrderFilterRequest request,
        CancellationToken ct)
    {
        var currentDay = DateTime.UtcNow;
        var ordersQuery = _context.Orders.AsQueryable();

        if (!string.IsNullOrEmpty(request.Query))
            ordersQuery = ordersQuery.Where(u =>
                EF.Functions.ILike(u.Email, $"%{request.Query}%") ||
                EF.Functions.ILike(u.Address, $"%{request.Query}%") ||
                EF.Functions.ILike(u.PhoneNumber, $"%{request.Query}%")
            );


        if (request.Period.HasValue && request.Period != TimePeriod.All)
        {
            var filterDate = request.Period switch
            {
                TimePeriod.Day => currentDay.AddDays(-1),
                TimePeriod.Week => currentDay.AddDays(-7),
                TimePeriod.Month => currentDay.AddMonths(-1),
                TimePeriod.Quarter => currentDay.AddMonths(-3),
                TimePeriod.Year => currentDay.AddYears(-1),
                _ => currentDay
            };

            ordersQuery = ordersQuery.Where(o => o.СreateDate >= filterDate);
        }

        if (request.Status.HasValue && request.Status != OrderStatuses.All)
            ordersQuery = request.Status switch
            {
                OrderStatuses.Pending => ordersQuery.Where(o => o.Status == OrderStatuses.Pending),
                OrderStatuses.InProgress => ordersQuery.Where(o => o.Status == OrderStatuses.InProgress),
                OrderStatuses.Completed => ordersQuery.Where(o => o.Status == OrderStatuses.Completed),
                OrderStatuses.Cancelled => ordersQuery.Where(o => o.Status == OrderStatuses.Cancelled),
                OrderStatuses.Failed => ordersQuery.Where(o => o.Status == OrderStatuses.Failed),
                _ => ordersQuery
            };

        ordersQuery = request.SortOrder switch
        {
            OrdersSortOrder.DateTimeAsc => ordersQuery.OrderBy(p => p.СreateDate),
            OrdersSortOrder.DateTimeDesc => ordersQuery.OrderByDescending(p => p.СreateDate),
            OrdersSortOrder.StatusAsc => ordersQuery.OrderBy(p => p.Status),
            OrdersSortOrder.StatusDesc => ordersQuery.OrderByDescending(p => p.Status),
            OrdersSortOrder.TotalPriceAsc => ordersQuery.OrderBy(p => p.TotalOrderPrice),
            OrdersSortOrder.TotalPriceDesc => ordersQuery.OrderByDescending(p => p.TotalOrderPrice),
            _ => ordersQuery.OrderBy(p => p.СreateDate)
        };

        var totalItems = await ordersQuery.CountAsync(ct);

        var orders = await ordersQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (orders, totalItems);
    }

    public async Task<Order> GetOrderById(int orderId, CancellationToken ct)
    {
        var order = await _context.Orders
            .Include(o => o.OrderProductList)
            .ThenInclude(o => o.OrderProductSnapshots)
            .FirstAsync(o => o.Id == orderId, ct);
        return order;
    }

    public async Task<IList<Order>> GetOrdersByUser(string userId, CancellationToken ct)
    {
        return await _context.Orders.Where(o => o.UserId == userId || o.GuestId == userId)
            .ToListAsync(ct);
    }

    public async Task<IList<OrderProductSnapshot>> GetOrderItemsById(int orderId, CancellationToken ct)
    {
        return await _context.OrderProductSnapshots.Where(o => o.Id == orderId).ToListAsync(ct);
    }

    public async Task UpdateOrder(Order order, CancellationToken ct)
    {
        _context.Update(order);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateOrderItems(IEnumerable<OrderProductSnapshot> orderItems,
        CancellationToken ct)
    {
        foreach (var item in orderItems)
        {
            var orderProduct = await _context.OrderProductSnapshots.FindAsync(item.Id, ct);
            if (orderProduct != null) orderProduct.Quantity = item.Quantity;
        }

        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateOrderProductListTotalPrice(int orderProductListId, decimal newTotalPrice,
        CancellationToken ct)
    {
        var orderProductList = await _context.OrderProductLists.FindAsync(orderProductListId, ct);
        if (orderProductList != null)
        {
            orderProductList.TotalPrice = newTotalPrice;
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task DeleteOrderProduct(Order order, int productId, CancellationToken ct)
    {
        var snapshotToRemove = order.OrderProductList.OrderProductSnapshots
            .FirstOrDefault(s => s.ProductId == productId);

        order.OrderProductList.OrderProductSnapshots.Remove(snapshotToRemove);

        await _context.SaveChangesAsync(ct);
    }

    public async Task<SalesMetricDto> GetSalesMetric(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var dayOfWeek = (int)now.DayOfWeek;
        dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
        var startOfWeek = now.Date.AddDays(1 - dayOfWeek);
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var startOfYear = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);


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
            .ToListAsync(ct);

        var salesMetric = new SalesMetricDto
        {
            Daily = sales.Where(x => x.Date == now.Date).Sum(x => x.Revenue),
            Weekly = sales.Where(x => x.Date >= startOfWeek).Sum(x => x.Revenue),
            Monthly = sales.Where(x => x.Date >= startOfMonth).Sum(x => x.Revenue),
            Yearly = sales.Where(x => x.Date >= startOfYear).Sum(x => x.Revenue),
            TotalOrders = new TotalOrderDto
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
                .Select(g => new TopProductDto
                {
                    Name = g.Key,
                    Sales = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.Sales)
                .Take(5)
                .ToListAsync(ct)
        };

        return salesMetric;
    }

    public async Task<decimal> GetRevenue(CancellationToken ct)
    {
        return await _context.Orders.AsNoTracking().SumAsync(o => o.TotalOrderPrice, ct);
    }

    public async Task<int> GetOrdersCount(CancellationToken ct)
    {
        return await _context.Orders.AsNoTracking().CountAsync(ct);
    }
}
