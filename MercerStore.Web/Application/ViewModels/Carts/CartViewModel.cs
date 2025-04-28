namespace MercerStore.Web.Application.ViewModels.Carts;

public class CartViewModel
{
    public int? CategoryId { get; set; }
    public IEnumerable<CartProductViewModel>? CartItems { get; set; }
    public int CartItemCount { get; set; }
    public decimal CartTotalPrice { get; set; }
}