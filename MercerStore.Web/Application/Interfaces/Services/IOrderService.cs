using MercerStore.Web.Application.Dtos.OrderDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Requests.Orders;
using MercerStore.Web.Application.ViewModels;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<PaginatedResultDto<OrderDto>> GetFilteredOrders(OrderFilterRequest request);
        Task<Order> GetOrderById(int orderId);
        Task<IEnumerable<Order>> GetOrderByUserId(string userId);
        Task<Order> AddOrder(Order order);
        Task UpdateOrderStatus(UpdateOrderStatusDto dto);
        Task RemoveOrder(int orderId);
        Task RemoveOrderProduct(int orderId, int productId);
        Task<OrderViewModel> GetOrderViewModel();
        Task<int> CreateOrderFromCart(OrderViewModel orderViewModel);
        Task<UpdateOrderViewModel> GetUpdateOrderViewModel(int orderId);
        Task UpdateOder(UpdateOrderViewModel updateOrderViewModel);

    }
}
