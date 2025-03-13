using MercerStore.Web.Infrastructure.Data.Enum.Order;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.ViewModels.Orders
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
