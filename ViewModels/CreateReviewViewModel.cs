using MercerStore.Models;
using System.ComponentModel.DataAnnotations;

namespace MercerStore.ViewModels
{
    public class CreateReviewViewModel
    {
        public int productId { get; set; }
        public string? ReviewText { get; set; }
        [Required(ErrorMessage = "Оценка обязательна")]

        public int Value { get; set; }
    }
}
