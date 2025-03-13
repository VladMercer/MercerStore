using MercerStore.Web.Application.Models.Carts;
using MercerStore.Web.Application.ViewModels.Carts;

namespace MercerStore.Web.Application.Interfaces.Repositories
{
    public interface ICartProductRepository
    {
        Task AddToCartProduct(int productId, string userId, int quantity);
        Task RemoveFromCartProduct(int productId, string userId);
        Task<List<CartProduct>> GetCartItems(string userId);
        Task<int> GetCartItemCount(string userId);
        Task<Cart> GetCartForUserId(string userId);
    }
}
