using MercerStore.Data.Enum.Product;
using MercerStore.Dtos.ProductDto;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ISKUService _skuService;
        public ProductsController(IProductRepository productRepository, ISKUService skuService)
        {
            _productRepository = productRepository;
            _skuService = skuService;
        }

        /// <summary>
        /// Получить фильтрованные продукты.
        /// </summary>
        /// <param name="categoryId">ID категории</param>
        /// <param name="sortOrder">Сортировки</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Количество продуктов на странице</param>
        /// <param name="filter">фильтры</param>
        /// <returns>Список продуктов</returns>

        [HttpGet("products/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllProducts(int? categoryId, int pageNumber, int pageSize, AdminProductSortOrder? sortOrder, AdminProductFilter? filter)
        {
            filter ??= AdminProductFilter.All;
            sortOrder ??= AdminProductSortOrder.NameAsc;

            var (products, totalItems) = await _productRepository
            .GetProductsAsync(categoryId, sortOrder.Value, filter.Value, pageNumber, pageSize);

            var pageProducts = products
            .Select(p => new AdminProductDto
                {
                Id = p.Id,
                Name = p.Name,
                Price = p.ProductPricing.OriginalPrice,
                MainImageUrl = p.MainImageUrl,
                CategoryId = p.CategoryId,
                Description = p.ProductDescription.DescriptionText,
                DiscountedPrice = p.ProductPricing.DiscountedPrice,
                DiscountEnd = p.ProductPricing.DiscountEnd,
                DiscountStart = p.ProductPricing.DiscountStart,
                InStock = p.ProductStatus.InStock,
                RemainingDiscountDays = p.ProductPricing.RemainingDiscountDays,
                Status = p.ProductStatus.Status switch
                {
                    ProductStatuses.Available => "В наличии",
                    ProductStatuses.OutOfStock => "Нет в наличии",
                    ProductStatuses.Archived => "Снят с продажи",
                    _ => "Неизвестный статус"
                }
            });

            var result = new
            {
                Products = pageProducts,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
            };

            return Ok(result);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return Ok(product);
        }
        [HttpGet("product/sku")]
        public async Task<IActionResult> GetProductSKU(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            var Sku = _skuService.GenerateSKU(product);
            return Ok(Sku);
        }
    }
}

