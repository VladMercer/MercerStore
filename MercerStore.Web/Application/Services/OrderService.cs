using MercerStore.Web.Application.Dtos.Order;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Application.ViewModels;
using MercerStore.Web.Application.ViewModels.Carts;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;
using MercerStore.Web.Infrastructure.Data.Enum.Order;

namespace MercerStore.Web.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userProfileRepository;


    public OrderService(IOrderRepository orderRepository, IUserRepository userProfileRepository)
    {
        _orderRepository = orderRepository;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<PaginatedResultDto<OrderDto>> GetFilteredOrdersWithoutCache(OrderFilterRequest request,
        CancellationToken ct)
    {
        var (orders, totalItems) = await _orderRepository.GetFilteredOrders(request, ct);

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
            CompletedDate = order.CompletedDate
        }).ToList();

        return new PaginatedResultDto<OrderDto>(orderDtos, totalItems, request.PageSize);
    }

    public async Task<Order> GetOrderById(int orderId, CancellationToken ct)
    {
        return await _orderRepository.GetOrderById(orderId, ct);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserId(string userId, CancellationToken ct)
    {
        return await _orderRepository.GetOrdersByUser(userId, ct);
    }

    public async Task<Order> AddOrder(Order order, CancellationToken ct)
    {
        return await _orderRepository.AddOrder(order, ct);
    }

    public async Task UpdateOrderStatus(UpdateOrderStatusDto dto, CancellationToken ct)
    {
        var order = await _orderRepository.GetOrderById(dto.OrderId, ct);
        order.Status = dto.Status;
        await _orderRepository.UpdateOrder(order, ct);
    }

    public async Task RemoveOrder(int orderId, CancellationToken ct)
    {
        await _orderRepository.DeleteOrder(orderId, ct);
    }

    public async Task RemoveOrderProduct(int orderId, int productId, CancellationToken ct)
    {
        var order = await _orderRepository.GetOrderById(orderId, ct);
        await _orderRepository.DeleteOrderProduct(order, productId, ct);
    }

    public async Task<OrderViewModel> GetOrderViewModel(string userId, CartViewModel cartViewModel,
        CancellationToken ct)
    {
        var user = await _userProfileRepository.GetUserByIdAsyncNoTracking(userId, ct);

        return new OrderViewModel
        {
            Address = user?.Address,
            Email = user?.Email,
            CartViewModel = cartViewModel,
            PhoneNumber = user?.PhoneNumber
        };
    }

    public async Task<Order> CreateOrderFromCart(OrderViewModel orderViewModel, string currentUserId,
        IEnumerable<string> roles, CancellationToken ct)
    {
        var isGuest = roles.Contains("Guest", StringComparer.Ordinal);

        var guestId = isGuest ? currentUserId : null;
        var userId = isGuest ? null : currentUserId;

        return await _orderRepository.CreateOrderFromCart(userId, guestId, orderViewModel, ct);
    }

    public async Task<UpdateOrderViewModel> GetUpdateOrderViewModel(int orderId, CancellationToken ct)
    {
        var order = await _orderRepository.GetOrderById(orderId, ct);
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

    public async Task UpdateOder(UpdateOrderViewModel updateOrderViewModel, CancellationToken ct)
    {
        var updatedTotalPrice = updateOrderViewModel.OrderItems.Sum(o => o.Quantity * o.PriceAtOrder);

        if (updateOrderViewModel is { Status: OrderStatuses.Completed, CompletedDate: null })
            updateOrderViewModel.CompletedDate = DateTime.UtcNow;

        var order = await _orderRepository.GetOrderById(updateOrderViewModel.Id, ct);

        order.Address = updateOrderViewModel.Address;
        order.CompletedDate = updateOrderViewModel.CompletedDate;
        order.Email = updateOrderViewModel.Email;
        order.TotalOrderPrice = updatedTotalPrice;
        order.PhoneNumber = updateOrderViewModel.PhoneNumber;
        order.Status = updateOrderViewModel.Status;

        await _orderRepository.UpdateOrder(order, ct);

        var updateOrderItems = updateOrderViewModel.OrderItems.Select(o => new OrderProductSnapshot
        {
            Id = o.Id,
            Quantity = o.Quantity
        }).ToList();

        await _orderRepository.UpdateOrderItems(updateOrderItems, ct);

        await _orderRepository.UpdateOrderProductListTotalPrice(updateOrderViewModel.OrderProductListId,
            updatedTotalPrice, ct);
    }
}
