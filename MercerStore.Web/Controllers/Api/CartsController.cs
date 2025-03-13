using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserIdentifierService _userIdentifierService;

        public CartsController(ICartService cartService, IUserIdentifierService userIdentifierService)
        {
            _cartService = cartService;
            _userIdentifierService = userIdentifierService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
          
            var cartViewModel = await _cartService.GetCartViewModel();
            return Ok(cartViewModel);
        }

        [HttpGet("itemCount")]
        public async Task<IActionResult> GetCartItemCount()
        {
            
            var itemCount = await _cartService.GetCartItemCount();
            return Ok(itemCount);
        }

        [HttpPost("product/{productId}")]
        [LogUserAction("User added item to cart", "product")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            await _cartService.AddToCart(productId);
            return Ok(productId);
        }

        [HttpDelete("product/{productId}")]
        [LogUserAction("User removed item from cart", "product")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            await _cartService.RemoveFromCart(productId);
            return Ok(productId);
        }
    }
}
