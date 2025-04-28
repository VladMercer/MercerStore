namespace MercerStore.Web.Application.Dtos.Order;

public class OrderDto
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string Address { get; set; }
    public string? UserId { get; set; }
    public string? GuestId { get; set; }
    public decimal TotalOrderPrice { get; set; }
    public string Status { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? CompletedDate { get; set; }
}