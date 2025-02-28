using MercerStore.Helpers;
using MercerStore.Models.Orders;
using MercerStore.Models.Products;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MercerStore.Areas.Admin.ViewModels
{
    public class UpdateUserProfileViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Никнейм")]
        public string UserName { get; set; }
        [Display(Name = "Почта")]
        [Required(ErrorMessage = "Почта обязательна")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Дата создания")]
        [ModelBinder(BinderType = typeof(DateTimeModelBinder))]
        public DateTime? CreateDate { get; set; }
        [Display(Name = "Последняя активность")]
        [ModelBinder(BinderType = typeof(DateTimeModelBinder))]
        public DateTime? LastActivityDate { get; set; }

        [Display(Name = "Фото")]
        public string? UserImgUrl { get; set; }
        public string? DefaultUserImgUrl { get; set; }
        [Display(Name = "Адрес")]
        public string? Address { get; set; }
        [Display(Name = "Количество отзывов")]
        public int? CountReviews { get; set; }
        [Display(Name = "Количество заказов")]
        public int? CountOrders { get; set; }
        [Display(Name = "Роли")]
        public List<Order?> Orders { get; set; } = [];
        public List<Review?> Reviews { get; set; } = [];
        public List<string?> Roles { get; set; } = [];
    }
}
