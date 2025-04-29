using System.ComponentModel.DataAnnotations;
using MercerStore.Web.Infrastructure.Data.Enum.Product;

namespace MercerStore.Web.Areas.Admin.ViewModels.Products;

public class UpdateProductViewModel
{
    public int Id { get; set; }

    [Display(Name = "Статус продукта")] public ProductStatuses Status { get; set; }

    [Display(Name = "Название продукта")] public string ProductName { get; set; }

    [Display(Name = "Стоимость")] public decimal Price { get; set; }

    [Display(Name = "Стоимость со скидкой")]
    public decimal? DiscountPrice { get; set; }

    [Display(Name = "Процент скидки")] public decimal? DiscountPercentage { get; set; }

    [Display(Name = "Длительность скидки")]
    public int? RemainingDiscountDays { get; set; }

    [Display(Name = "Начало скидки")] public DateTime? DiscountStart { get; set; }

    [Display(Name = "Конец скидки")] public DateTime? DiscountEnd { get; set; }

    [Display(Name = "Описание")] public string Description { get; set; }

    [Display(Name = "Загрузить новый файл картинки продукта")]
    public IFormFile? MainImage { get; set; }

    [Display(Name = "Ссылка картинки продукта")]
    public string? MainImageUrl { get; set; }

    [Display(Name = "Количество на складе")]
    public int InStock { get; set; }
}
