using MercerStore.Models;
using System.ComponentModel.DataAnnotations;

namespace MercerStore.ViewModels
{
	public class OrderViewModel
	{
        public CartViewModel? CartViewModel { get; set; }
		[Display(Name = "Email")]
		[DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите корректный email-адрес")]
        public string? Email { get; set; }

		[Display(Name = "Номер телефона")]
		[Required(ErrorMessage = "Это обязательное поле")]
		public string? PhoneNumber { get; set; }
		[Display(Name = "Адресс")]
		[Required(ErrorMessage = "Адресс обязателен")]
		public string? Address { get; set; }
    }
}
