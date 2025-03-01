using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartProductRepository _cartProductRepository;
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly IRequestContextService _requestContextService;
        public CartsController(ICartProductRepository cartProductRepository,
                               IUserIdentifierService userIdentifierService,
                               IRequestContextService requestContextService)
        {

            _cartProductRepository = cartProductRepository;
            _userIdentifierService = userIdentifierService;
            _requestContextService = requestContextService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {

            var userId = _userIdentifierService.GetCurrentIdentifier();
            var cartViewModel = await _cartProductRepository.GetCartViewModel(userId);
            return Ok(cartViewModel);
        }
        [HttpGet("itemCount")]
        public async Task<IActionResult> GetCartItemCount()
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var itemCount = await _cartProductRepository.GetCartItemCount(userId);
            return Ok(itemCount);
        }
        [HttpPost("product/{productId}")]
        [LogUserAction("User added item to cart", "product")]
        public async Task<IActionResult> AddToCart(int productId)
        {

            var quantity = 1;
            var userId = _userIdentifierService.GetCurrentIdentifier();
            await _cartProductRepository.AddToCartProduct(productId, userId, quantity);
            var logDetails = new
            {
                Quantity = quantity
            };
            _requestContextService.SetLogDetails(logDetails);
            return Ok(productId);
        }

        [HttpDelete("product/{productId}")]
        [LogUserAction("User removed item from cart", "product")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            await _cartProductRepository.RemoveFromCartProduct(productId, userId);

            return Ok(productId);
        }
    }
}
