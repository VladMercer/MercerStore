using MercerStore.Dtos.ProductDto;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Authorize]
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
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

            var result = new
            {
                Products = pageProducts,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
            };

            return Ok(result);
        }

        [HttpGet("price-range/{categoryId}")]
        public async Task<IActionResult> GetPriceRange(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);

            var minPrice = products
                .Select(p => p.ProductPricing.FixedDiscountPrice
                             ?? p.ProductPricing.DiscountedPrice
                             ?? p.ProductPricing.OriginalPrice)
                .Min();

            var maxPrice = products
                .Select(p => p.ProductPricing.FixedDiscountPrice
                             ?? p.ProductPricing.DiscountedPrice
                             ?? p.ProductPricing.OriginalPrice)
                .Max();

            var result = new
            {
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            return Ok(result);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}