using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAdminFilteredProducts([FromQuery] ProductFilterRequest request)
        {
            var result = await _productService.GetAdminFilteredProducts(request);
            return Ok(result);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _productService.GetProduct(productId);
            return Ok(product);
        }

        [HttpGet("product/sku")]
        public async Task<IActionResult> GetProductSku(int productId)
        {
            var Sku = await _productService.GetProductSku(productId);
            return Ok(Sku);
        }
    }
}

