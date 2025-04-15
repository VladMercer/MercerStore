using MercerStore.Web.Application.ViewModels.Carts;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartViewModel> GetCartViewModel(string userId);
        Task<int?> GetCartItemCount(string userId);
        Task AddToCart(int productId, string userId);
        Task RemoveFromCart(int productId, string userId);
    }
}
