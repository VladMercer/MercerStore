using System.ComponentModel.DataAnnotations;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Areas.Admin.ViewModels.Products;

public class CreateProductViewModel
{
    [Display(Name = "Название")] public string Name { get; set; }

    [Display(Name = "Стоимость")] public decimal Price { get; set; }

    [Display(Name = "Описание")] public string Description { get; set; }

    [Display(Name = "Фото продукта")] public IFormFile? MainImage { get; set; }

    [Display(Name = "Категория")] public int CategoryId { get; set; }

    [Display(Name = "Количество товара")] public int InStock { get; set; }

    public IEnumerable<Category>? Categories { get; set; }
}