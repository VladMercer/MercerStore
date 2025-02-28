using MercerStore.Data.Enum.Order;
using MercerStore.Helpers;
using MercerStore.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Areas.Admin.ViewModels
{
    public class UpdateOrderViewModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }
        public string? UserId { get; set; }
        public string? GuestId { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public int OrderProductListId { get; set; }
        public OrderStatuses Status { get; set; }
        [ModelBinder(BinderType = typeof(DateTimeModelBinder))]
        public DateTime CreateDate { get; set; }
        [ModelBinder(BinderType = typeof(DateTimeModelBinder))]
        public DateTime? CompletedDate { get; set; }
        public List<OrderProductSnapshotViewModel> OrderItems { get; set; }
    }
}
