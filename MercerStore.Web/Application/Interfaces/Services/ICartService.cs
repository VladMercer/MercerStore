using MercerStore.Web.Application.ViewModels.Carts;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface ICartService
{
    Task<CartViewModel> GetCartViewModel(string userId, CancellationToken ct);
    Task<int?> GetCartItemCount(string userId, CancellationToken ct);
    Task AddToCart(int productId, string userId, CancellationToken ct);
    Task RemoveFromCart(int productId, string userId, CancellationToken ct);
}
