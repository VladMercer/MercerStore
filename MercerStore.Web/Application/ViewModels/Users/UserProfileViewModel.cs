using System.ComponentModel.DataAnnotations;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.ViewModels.Users;

public class UserProfileViewModel
{
    public string Id { get; set; }

    [Display(Name = "Никнейм")] public string UserName { get; set; }

    [Display(Name = "Почта")]
    [Required(ErrorMessage = "Почта обязательна")]
    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; }

    [Display(Name = "Номер телефона")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Фото")] public IFormFile? UserImage { get; set; }

    public string? UserImgUrl { get; set; }

    [Display(Name = "Адрес")] public string? Address { get; set; }

    public IList<Order?> Orders { get; set; } = [];

    public IList<Review?> Reviews { get; set; } = [];

    [Display(Name = "Дата создания")] public DateTime? CreateDate { get; set; }

    [Display(Name = "Количество отзывов")] public int? CountReviews { get; set; }

    [Display(Name = "Количество заказов")] public int? CountOrders { get; set; }
}