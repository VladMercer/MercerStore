using System.ComponentModel.DataAnnotations;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Areas.Admin.ViewModels.Users;

public class UpdateUserProfileViewModel
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

    [Display(Name = "Дата создания")] public DateTime? CreateDate { get; set; }

    [Display(Name = "Последняя активность")]

    public DateTime? LastActivityDate { get; set; }

    [Display(Name = "Фото")] public string? UserImgUrl { get; set; }

    public string? DefaultUserImgUrl { get; set; }

    [Display(Name = "Адрес")] public string? Address { get; set; }

    [Display(Name = "Количество отзывов")] public int? CountReviews { get; set; }

    [Display(Name = "Количество заказов")] public int? CountOrders { get; set; }

    [Display(Name = "Роли")] public IList<Order?> Orders { get; set; } = [];

    public IList<Review?> Reviews { get; set; } = [];
    public IList<string?> Roles { get; set; } = [];
}
