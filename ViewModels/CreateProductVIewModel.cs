using MercerStore.Models;
using MercerStore.Models.DescriptionProducts;
using System.ComponentModel.DataAnnotations;

namespace MercerStore.Models.ViewModels
{
    public class CreateProductViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Стоимость")]
        public decimal Price { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Фото продукта")]
        public IFormFile? MainImage { get; set; }
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
       
    }
}