public class CartViewModel
{
	public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
	public decimal TotalPrice { get; set; }
	public decimal Discount { get; set; }
	public decimal ShippingCost { get; set; }
	public decimal GrandTotal => TotalPrice - Discount + ShippingCost;
}