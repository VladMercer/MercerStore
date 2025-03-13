using MercerStore.Web.Application.ViewModels.Carts;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartViewModel> GetCartViewModel();
        Task<int?> GetCartItemCount();
        Task AddToCart(int productId);
        Task RemoveFromCart(int productId);
    }
}
