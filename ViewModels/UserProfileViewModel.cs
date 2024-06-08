using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace MercerStore.ViewModels
{
    public class UserProfileViewModel
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
        [Display(Name = "Фото")]
        public IFormFile? UserImage { get; set; }
        public string? UserImgUrl { get; set; }
        public string DefaultUserImgUrl { get; set; } = "/img/Default/ppHwy5fH-lg.jpg";
        [Display(Name = "Адрес")]
        public string? Address { get; set; }
    }
}