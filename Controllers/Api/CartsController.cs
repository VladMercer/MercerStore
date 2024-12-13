using MercerStore.Extentions;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Route("api/cart")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartProductRepository _cartProductRepository;
        public CartsController(ICartProductRepository cartProductRepository)
        {

            _cartProductRepository = cartProductRepository;

        }
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {

            var userId = User.GetUserId();
            var cartViewModel = await _cartProductRepository.GetCartViewModel(userId);
            return Ok(cartViewModel);

        }
        [HttpGet("itemCount")]
        public async Task<IActionResult> GetCartItemCount()
        {
            var userId = User.GetUserId();
            var itemCount = await _cartProductRepository.GetCartItemCount(userId);
            return Ok(itemCount);
        }
        [HttpPost("product/{productId}")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var quantity = 1;
            var userId = User.GetUserId();
            await _cartProductRepository.AddToCartProduct(productId, userId, quantity);
            return Ok();
        }
        
        [HttpDelete("product/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = User.GetUserId();
            await _cartProductRepository.RemoveFromCartProduct(productId, userId);

            return Ok();
        }
    }
}
