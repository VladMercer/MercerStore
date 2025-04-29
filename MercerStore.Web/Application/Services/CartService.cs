using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Carts;

namespace MercerStore.Web.Application.Services;

public class CartService : ICartService
{
    private readonly ICartProductRepository _cartProductRepository;

    public CartService(ICartProductRepository cartProductRepository)
    {
        _cartProductRepository = cartProductRepository;
    }

    public async Task<CartViewModel> GetCartViewModel(string userId, CancellationToken ct)
    {
        var cartItems = await _cartProductRepository.GetCartItems(userId, ct);

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

    public async Task<int?> GetCartItemCount(string userId, CancellationToken ct)
    {
        return await _cartProductRepository.GetCartItemCount(userId, ct);
    }

    public async Task AddToCart(int productId, string userId, CancellationToken ct)
    {
        const int quantity = 1;
        await _cartProductRepository.AddToCartProduct(productId, userId, quantity, ct);
    }

    public async Task RemoveFromCart(int productId, string userId, CancellationToken ct)
    {
        await _cartProductRepository.RemoveFromCartProduct(productId, userId, ct);
    }
}
