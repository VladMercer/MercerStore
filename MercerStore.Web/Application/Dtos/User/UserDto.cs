namespace MercerStore.Web.Application.Dtos.User;

public class UserDto
{
    public string Id { get; set; }
    public string ImageUrl { get; set; }
    public string Email { get; set; }
    public string? Address { get; set; }


    public DateTime CreateDate { get; set; }


    public DateTime? LastActivityDate { get; set; }

    public string? PhoneNumber { get; set; }
    public int? CountReviews { get; set; }
    public int? CountOrders { get; set; }
    public IList<string>? Roles { get; set; }
}
