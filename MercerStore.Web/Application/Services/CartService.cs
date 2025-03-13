using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Carts;

namespace MercerStore.Web.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartProductRepository _cartProductRepository;
        private readonly IRequestContextService _requestContextService;
        private readonly IUserIdentifierService _userIdentifierService;

        private string UserId => _userIdentifierService.GetCurrentIdentifier();

        public CartService(
            ICartProductRepository cartProductRepository,
            IRequestContextService requestContextService,
            IUserIdentifierService userIdentifierService)
        {
            _cartProductRepository = cartProductRepository;
            _requestContextService = requestContextService;
            _userIdentifierService = userIdentifierService;
        }
        public async Task<CartViewModel> GetCartViewModel()
        {
            var cartItems = await _cartProductRepository.GetCartItems(UserId);

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
        public async Task<int?> GetCartItemCount()
        {
            return await _cartProductRepository.GetCartItemCount(UserId);
        } 
        public async Task AddToCart(int productId)
        {
            var quantity = 1;

            await _cartProductRepository.AddToCartProduct(productId, UserId, quantity);

            var logDetails = new
            {
                quantity,
                UserId,
            };

            _requestContextService.SetLogDetails(logDetails);
        }

        public async Task RemoveFromCart(int productId)
        {
            await _cartProductRepository.RemoveFromCartProduct(productId, UserId);
            var logDetails = new
            {
                UserId,
                productId
            };

            _requestContextService.SetLogDetails(logDetails);
        }
    }
}
