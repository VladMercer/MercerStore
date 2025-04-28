using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.ViewModels.Products;

public class ProductViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? SKU { get; set; }
    public string MainImageUrl { get; set; }
    public string Description { get; set; }
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
    public string Status { get; set; }
    public decimal? DiscountPrice { get; set; }
}
