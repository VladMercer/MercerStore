using MercerStore.Data;
using MercerStore.Interfaces;
using MercerStore.Models;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Repository
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

            var cart = await _context.Carts.SingleOrDefaultAsync(c => c.AppUserId == userId);
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

        public async Task<CartViewModel> GetCartViewModel(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .SingleOrDefaultAsync(c => c.AppUserId == userId);

            if (cart != null)
            {
                var cartItems = cart.CartProducts.Select(cp => new CartProductViewModel
                {
                    ProductId = cp.ProductId,
                    Name = cp.Product.Name,
                    ImageUrl = cp.Product.MainImageUrl,
                    Price = cp.Product.Price,
                    Quantity = cp.Quantity
                }).ToList();

                var totalQuantity = cartItems.Sum(ci => ci.Quantity);
                var totalPrice = cartItems.Sum(ci => ci.Price * ci.Quantity);

                return new CartViewModel
                {
                    CartItems = cartItems,
                    CartItemCount = totalQuantity,
                    CartTotalPrice = totalPrice
                };
            }
            return new CartViewModel
            {
                CartItems = new List<CartProductViewModel>(),
                CartItemCount = 0,
                CartTotalPrice = 0m
            };
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
                _context.CartProducts.Remove(cartItem);
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
    }
}
