using System.ComponentModel.DataAnnotations;

namespace MercerStore.Web.Application.ViewModels.Users;

public class RegisterViewModel
{
    [Display(Name = "Почта")]
    [Required(ErrorMessage = "Введите почту")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Подтверждение пароля")]
    [Required(ErrorMessage = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
}
