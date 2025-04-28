using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Infrastructure.Data.Enum.Order;

namespace MercerStore.Web.Application.Models.Orders;

public class Order : IEntity
{
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string Address { get; set; }


    public string? UserId { get; set; }
    public AppUser? User { get; set; }
    public string? GuestId { get; set; }


    public int OrderProductListId { get; set; }
    public OrderProductList? OrderProductList { get; set; }

    public decimal TotalOrderPrice { get; set; }

    public OrderStatuses Status { get; set; }
    public DateTime СreateDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public int Id { get; set; }
}