using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class CartController : Controller
{
	private readonly IProductRepository _productRepository;

	public CartController(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<IActionResult> Index()
	{
		
		var products = await _productRepository.GetLastProductsAsync(3);

		
		var cartViewModel = new CartViewModel
		{
			Items = products.Select(p => new CartItemViewModel
			{
				ProductId = p.Id,
				Name = p.Name,
				ImageUrl = p.MainImageUrl,
				Price = p.Price,
				Quantity = 1
			}).ToList(),
			ShippingCost = 250, 
			Discount = 300 
		};

		cartViewModel.TotalPrice = cartViewModel.Items.Sum(i => i.Price * i.Quantity);

		return View(cartViewModel);
	}
}