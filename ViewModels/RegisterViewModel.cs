using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace MercerStore.ViewModels
{
	public class RegisterViewModel
	{
		[Display(Name ="Почта")]
		[Required(ErrorMessage ="Введите почту")]
		public string Email { get; set; }
		[Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
		
		[Display(Name ="Подтверждение пароля")]
		[Required(ErrorMessage ="Подтвердите пароль")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		public string ConfirmPassword { get; set; }
    }
}
