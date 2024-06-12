using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MercerStore.Models.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        [Display(Name = "Фото категории")]
        public IFormFile? CategoryImage { get; set; }
    }
}