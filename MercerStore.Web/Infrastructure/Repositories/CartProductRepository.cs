using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Carts;
using MercerStore.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories
{
    public class CartProductRepository : ICartProductRepository
    {
        private readonly AppDbContext _context;

        public CartProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartProduct(int productId, string userId, int quantity)
        {

            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                Console.WriteLine("Продукт не найден.");
                throw new InvalidOperationException("Продукт не найден.");
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.AppUserId == userId);
            if (cart == null)
            {
                cart = new Cart { AppUserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartProduct = await _context.CartProducts
                .FirstOrDefaultAsync(cp => cp.CartId == cart.Id && cp.ProductId == productId);

            if (cartProduct == null)
            {
                cartProduct = new CartProduct
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.CartProducts.Add(cartProduct);
            }
            else
            {
                cartProduct.Quantity += quantity;
                _context.CartProducts.Update(cartProduct);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

                Console.WriteLine($"Ошибка обновления базы данных: {ex.Message}");
                throw;
            }
        }

        public async Task<List<CartProduct>> GetCartItems(string userId)
        {
            var cart = await _context.Carts
                .AsNoTracking()
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .ThenInclude(p => p.ProductPricing)
                .SingleOrDefaultAsync(c => c.AppUserId == userId);

            return cart?.CartProducts.ToList() ?? new List<CartProduct>();
        }

        public async Task RemoveFromCartProduct(int productId, string userId)
        {
            var cart = await _context.Carts.SingleOrDefaultAsync(c => c.AppUserId == userId);
            if (cart == null)
            {
                return;
            }

            var cartItem = await _context.CartProducts.SingleOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == productId);
            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {

                    cartItem.Quantity -= 1;
                }
                else
                {

                    _context.CartProducts.Remove(cartItem);
                }

                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> GetCartItemCount(string userId)
        {
            var cart = await _context.Carts.SingleOrDefaultAsync(c => c.AppUserId == userId);
            if (cart != null)
            {
                return await _context.CartProducts.Where(ci => ci.CartId == cart.Id).SumAsync(ci => ci.Quantity);
            }
            return 0;
        }

        public async Task<Cart> GetCartForUserId(string userId)
        {
            return await _context.Carts.FirstAsync(u => u.AppUserId == userId);
        }
    }
}
