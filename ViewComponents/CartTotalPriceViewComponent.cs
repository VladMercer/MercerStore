using MercerStore.Extentions;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.ViewComponents
{
    public class CartTotalPriceViewComponent : ViewComponent
    {
        private readonly ICartProductRepository _cartProductRepository;

        public CartTotalPriceViewComponent(ICartProductRepository cartProductRepository)
        {
            _cartProductRepository = cartProductRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = HttpContext.User.GetUserId();
                var cartViewModel = await _cartProductRepository.GetCartViewModel(user);
                return View(cartViewModel);
            }
            else
            {
				return View(new CartViewModel
				{
					CartItems = new List<CartProductViewModel>(),
					CartItemCount = 0,
					CartTotalPrice = 0m
				});
			}
        }
    }
}
