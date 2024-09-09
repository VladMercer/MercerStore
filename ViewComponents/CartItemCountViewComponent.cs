using MercerStore.Extentions;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.ViewComponents
{
    public class CartItemCountViewComponent : ViewComponent
    {
        private readonly ICartProductRepository _cartProductRepository;
        public CartItemCountViewComponent(ICartProductRepository cartProductRepository)
        {
            _cartProductRepository = cartProductRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = HttpContext.User.GetUserId();
                var cartItemsCount = await _cartProductRepository.GetCartItemCount(user);
                return View(cartItemsCount);
            }
            else
            {
                return View(0);
            }
        }
    }
}
