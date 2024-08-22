using MercerStore;
using MercerStore.Interfaces;
using MercerStore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.ViewComponents
{
	public class ProductCartViewComponent : ViewComponent 
	{
		private readonly ICartProductRepository _cartProductRepository;

		public ProductCartViewComponent(ICartProductRepository cartProductRepository)
		{
			_cartProductRepository = cartProductRepository;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				var userId = HttpContext.User.GetUserId();
				var cartViewModel = await _cartProductRepository.GetCartViewModel(userId);
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
