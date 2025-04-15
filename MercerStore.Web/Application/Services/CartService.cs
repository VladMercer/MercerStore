using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Carts;

namespace MercerStore.Web.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartProductRepository _cartProductRepository;

        public CartService(ICartProductRepository cartProductRepository)
        {
            _cartProductRepository = cartProductRepository;
        }
        public async Task<CartViewModel> GetCartViewModel(string userId)
        {
            var cartItems = await _cartProductRepository.GetCartItems(userId);

            var cartProductViewModel = cartItems.Select(c => new CartProductViewModel
            {
                ProductId = c.ProductId,
                Name = c.Product.Name,
                ImageUrl = c.Product.MainImageUrl,
                Price = c.Product.ProductPricing.OriginalPrice,
                DiscountedPrice = c.Product.ProductPricing.DiscountedPrice,
                Quantity = c.Quantity
            }).ToList();

            var totalQuantity = cartProductViewModel.Sum(ci => ci.Quantity);
            var totalPrice = cartProductViewModel.Sum(ci => (ci.DiscountedPrice ?? ci.Price) * ci.Quantity);

            return new CartViewModel
            {
                CartItems = cartProductViewModel,
                CartItemCount = totalQuantity,
                CartTotalPrice = totalPrice
            };
        }
        public async Task<int?> GetCartItemCount(string userId)
        {
            return await _cartProductRepository.GetCartItemCount(userId);
        }
        public async Task AddToCart(int productId, string userId)
        {
            var quantity = 1;
            await _cartProductRepository.AddToCartProduct(productId, userId, quantity);
        }
        
        public async Task RemoveFromCart(int productId, string userId)
        {
            await _cartProductRepository.RemoveFromCartProduct(productId, userId);
        }
    }
}
