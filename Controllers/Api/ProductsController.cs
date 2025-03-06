using MercerStore.Data.Enum.Product;
using MercerStore.Dtos.OrderDto;
using MercerStore.Dtos.ProductDto;
using MercerStore.Dtos.ResultDto;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Text.Json;

namespace MercerStore.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ISKUService _skuService;
        private readonly IRedisCacheService _redisCacheService;
        public ProductsController(IProductRepository productRepository, ISKUService skuService, IRedisCacheService redisCacheService)
        {
            _productRepository = productRepository;
            _skuService = skuService;
            _redisCacheService = redisCacheService;
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
            bool isDefaultQuery =
                categoryId == null &&
                pageNumber == 1 &&
                pageSize == 30 &&
                !sortOrder.HasValue &&
                !filter.HasValue;

            string cacheKey = $"products:page1";
            if (isDefaultQuery)
            {
                var cachedData = await _redisCacheService.GetCacheAsync<string>(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    return Ok(JsonSerializer.Deserialize<object>(cachedData));
                }
            }
          

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

            var result = new PaginatedResultDto<AdminProductDto>(pageProducts, totalItems, pageSize);

            if (isDefaultQuery)
            {
                await _redisCacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            }

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

