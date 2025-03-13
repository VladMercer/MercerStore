using MercerStore.Web.Infrastructure.Data.Enum.Order;

namespace MercerStore.Web.Application.Dtos.OrderDto
{
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public OrderStatuses Status { get; set; }
    }
}
