
using MercerStore.Extentions;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Route("/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ICartProductRepository _cartProductRepository;
        public CartApiController(ICartProductRepository cartProductRepository)
        {

            _cartProductRepository = cartProductRepository;

        }
        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {

            var userId = User.GetUserId();
            var cartViewModel = await _cartProductRepository.GetCartViewModel(userId);
            return Ok(cartViewModel);

        }
		[HttpGet("GetItemCount")]
		public async Task<IActionResult> GetCartItemCount()
        {
            var userId = User.GetUserId();
            var itemCount = await _cartProductRepository.GetCartItemCount(userId);
            return Ok(itemCount);
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int quantity, int productId)
        {
            var userId = User.GetUserId();
            await _cartProductRepository.AddToCartProduct(productId, userId, quantity);
            return Ok();
        }
        
        [HttpPost("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = User.GetUserId();
            await _cartProductRepository.RemoveFromCartProduct(productId, userId);

            return Ok();
        }
    }
}
