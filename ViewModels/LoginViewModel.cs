using System.ComponentModel.DataAnnotations;

namespace MercerStore.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Почта")]
        [Required(ErrorMessage = "Email обязателен")]
        public string EmailAddress { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}