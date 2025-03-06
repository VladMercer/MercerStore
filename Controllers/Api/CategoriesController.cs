using MercerStore.Dtos.ProductDto;
using MercerStore.Dtos.ResultDto;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;



namespace MercerStore.Controllers.Api
{
    [Authorize]
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRedisCacheService _redisCacheService;
        public CategoriesController(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IRedisCacheService redisCacheService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _redisCacheService = redisCacheService;
        }

        /// <summary>
        /// Получить фильтрованные продукты по категории.
        /// </summary>
        /// <param name="categoryId">ID категории</param>
        /// <param name="sortOrder">Сортировка: "name_desc", "price_asc", "price_desc"</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Количество продуктов на странице</param>
        /// <param name="maxPrice">Максимальная цена</param>
        /// <param name="minPrice">Минимальная цена</param>
        /// <returns>Список продуктов</returns>

        [HttpGet("products/{categoryId}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetFilteredProducts(int categoryId, int pageNumber, int pageSize, string? sortOrder, int? maxPrice, int? minPrice)
         {
            var priceRange = await _categoryRepository.GetCategoryPriceRangeAsync(categoryId);

            bool isDefaultQuery =
            pageNumber == 1 &&
            pageSize == 9 &&
            string.IsNullOrEmpty(sortOrder) &&
            (minPrice.Value == priceRange.MinPrice) &&
            (maxPrice.Value == priceRange.MaxPrice);

            string cacheKey = $"products:{categoryId}:page1";

            if (isDefaultQuery)
            {
                var cachedData = await _redisCacheService.GetCacheAsync<string>(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    return Ok(JsonSerializer.Deserialize<object>(cachedData));
                }
            }

            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.ProductPricing.OriginalPrice >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.ProductPricing.OriginalPrice <= maxPrice.Value);
            }

            products = sortOrder switch
            {
                "name_desc" => products.OrderByDescending(p => p.Name),
                "price_asc" => products.OrderBy(p => p.ProductPricing.OriginalPrice),
                "price_desc" => products.OrderByDescending(p => p.ProductPricing.OriginalPrice),
                _ => products.OrderBy(p => p.Name),
            };

            int totalItems = products.Count();

            var pageProducts = products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.ProductPricing.OriginalPrice,
                MainImageUrl = p.MainImageUrl,
                CategoryId = p.CategoryId,
                Description = p.ProductDescription.DescriptionText,
                DiscountedPrice = p.ProductPricing.DiscountedPrice
            });

            var result = new PaginatedResultDto<ProductDto>(pageProducts, totalItems, pageSize);

            if (isDefaultQuery)
            {
                await _redisCacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            }

            return Ok(result);
        }

        [HttpGet("price-range/{categoryId}")]
        public async Task<IActionResult> GetPriceRange(int categoryId)
        {
            var priceRange = await _categoryRepository.GetCategoryPriceRangeAsync(categoryId);
            return Ok(priceRange);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}