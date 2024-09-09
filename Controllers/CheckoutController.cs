using MercerStore.Extentions;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
public class CheckoutController : Controller
{
    private readonly ICartProductRepository _cartProductRepository;

    public CheckoutController(ICartProductRepository cartProductRepository)
    {
        _cartProductRepository = cartProductRepository;
    }

    public async Task<IActionResult> Index()
    {

        var userId = User.GetUserId();
        var cartViewModel = await _cartProductRepository.GetCartViewModel(userId);
        return View(cartViewModel);
    }
}