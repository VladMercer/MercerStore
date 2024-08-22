using MercerStore;

using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class CartController : Controller
{
    private readonly ICartProductRepository _cartProductRepository;
    private readonly IProductRepository _productRepository;
    public CartController(ICartProductRepository cartProductRepository, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _cartProductRepository = cartProductRepository;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetUserId();
        var cartViewModel = await _cartProductRepository.GetCartViewModel(userId);
        return View(cartViewModel);
    }
    public async Task<IActionResult> GetCartItemCount()
    {
        var userId = User.GetUserId();
        var itemCount = await _cartProductRepository.GetCartItemCount(userId);
        return Json(itemCount);
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
    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId, string returnUrl)
    {
        var userId = User.GetUserId();
        await _cartProductRepository.RemoveFromCartProduct(productId, userId);

        if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Contains("Category"))
        {

            var categoryId = await _productRepository.GetCategoryByProductId(productId);

            if (categoryId.HasValue)
            {

                return RedirectToAction("Index", "Category", new { categoryId = categoryId.Value });
            }
        }
        if (!string.IsNullOrEmpty(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }
}
