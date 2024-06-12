using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;


public class CheckoutController : Controller
{
    private readonly IProductRepository _productRepository;

    public CheckoutController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Payment()
    {
        var recentProducts = await _productRepository.GetAllProductsAsync();
        var recentItems = recentProducts.OrderByDescending(p => p.Id).Take(3).ToList();

        var cartItems = recentItems.Select(p => new CartItemViewModel
        {
            ProductId = p.Id,
            Name = p.Name,
            ImageUrl = p.MainImageUrl,
            Price = p.Price,
            Quantity = 1 
        }).ToList();

        var totalPrice = cartItems.Sum(item => item.Price * item.Quantity);

        var cartViewModel = new CartViewModel
        {
            Items = cartItems,
            TotalPrice = totalPrice,
            Discount = 0, 
            ShippingCost = 0 
        };

        return View(cartViewModel);
    }
}