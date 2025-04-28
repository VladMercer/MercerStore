namespace MercerStore.Web.Application.ViewModels.Carts;

public class CartProductViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public int Quantity { get; set; }
}
