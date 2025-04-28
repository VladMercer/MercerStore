using MercerStore.Web.Application.Models.Carts;

namespace MercerStore.Web.Application.Interfaces.Repositories;

public interface ICartProductRepository
{
    Task AddToCartProduct(int productId, string userId, int quantity, CancellationToken ct);
    Task RemoveFromCartProduct(int productId, string userId, CancellationToken ct);
    Task<IEnumerable<CartProduct>> GetCartItems(string userId, CancellationToken ct);
    Task<int> GetCartItemCount(string userId, CancellationToken ct);
    Task<Cart> GetCartForUserId(string userId, CancellationToken ct);
}