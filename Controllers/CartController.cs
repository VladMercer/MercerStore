using MercerStore.Extentions;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class CartController : Controller
{
    private readonly ICartProductRepository _cartProductRepository;
    public CartController(ICartProductRepository cartProductRepository)
    {
        _cartProductRepository = cartProductRepository;

    }
    public async Task<IActionResult> Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddToCart(int quantity, int productId, string returnUrl)
    {
        var userId = User.GetUserId();
        await _cartProductRepository.AddToCartProduct(productId, userId, quantity);

        if (!string.IsNullOrEmpty(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction("Index", "Home");
    }
}
