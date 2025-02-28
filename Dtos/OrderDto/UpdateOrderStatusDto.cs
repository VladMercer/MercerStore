using MercerStore.Data.Enum.Order;

namespace MercerStore.Dtos.OrderDto
{
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public OrderStatuses Status { get; set; }
    }
}
