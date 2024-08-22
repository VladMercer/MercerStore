using MercerStore.Models;

namespace MercerStore.Interfaces
{
    public interface ICartProductRepository
    {
        Task AddToCartProduct(int productId, string userId, int quantity);
        Task RemoveFromCartProduct(int productId, string userId);
        Task<CartViewModel> GetCartViewModel(string userId);
        Task<int> GetCartItemCount(string userId);
    }
}
