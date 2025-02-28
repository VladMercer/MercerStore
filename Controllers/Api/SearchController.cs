using MercerStore.Interfaces;
using MercerStore.Models.Products;
using MercerStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Authorize]
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IProductRepository _productRepository;
        public SearchController(IElasticSearchService elasticSearchService, IProductRepository productRepository)
        {
            _elasticSearchService = elasticSearchService;
            _productRepository = productRepository;
        }
        /// <summary>
        /// Найти и отфильтровать продукты.
        /// </summary>
        /// <param name="query">запрос</param>
        /// <param name="sortOrder">Сортировка: "name_asc", "name_desc", "price_asc", "price_desc"</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Количество продуктов на странице</param>
        /// <returns>Список найденных продуктов</returns>
        [HttpGet("search")]
        public async Task<IActionResult> Search(string query, string? sortOrder, int? pageNumber, int? pageSize)
        {

            var productIndexDto = await _elasticSearchService.SearchProductsAsync(query);

            var productIds = productIndexDto.Select(x => x.Id).ToList();

            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            products = sortOrder switch
            {
                "price_asc" => products.OrderBy(p => p.ProductPricing.OriginalPrice).ToList(),
                "price_desc" => products.OrderByDescending(p => p.ProductPricing.OriginalPrice).ToList(),
                "name_asc" => products.OrderBy(p => p.Name).ToList(),
                "name_desc" => products.OrderByDescending(p => p.Name).ToList(),
                _ => products 
            };
            var totalItems = products.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                products = products.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
            }

            var productIndexDict = productIndexDto.ToDictionary(dto => dto.Id);

            var productViewModels = products
            .Select(p =>
            {
                productIndexDict.TryGetValue(p.Id, out var indexDto);

                return new SearchProductViewModel
                {
                    Id = p.Id,
                    Name = indexDto.Name,
                    Description = p.ProductDescription.DescriptionText,
                    Price = p.ProductPricing.OriginalPrice,
                    MainImageUrl = p.MainImageUrl,
                    CategoryId = p.CategoryId,
                    InStock = p.ProductStatus.InStock,
                    DiscountedPrice = p.ProductPricing.DiscountedPrice,
                    DiscountEnd = p.ProductPricing.DiscountEnd,
                    DiscountStart = p.ProductPricing.DiscountStart,
                    RemainingDiscountDays = p.ProductPricing.RemainingDiscountDays,
                    Status = p.ProductStatus.Status switch
                    {
                        ProductStatuses.Available => "В наличии",
                        ProductStatuses.OutOfStock => "Нет в наличии",
                        ProductStatuses.Archived => "Снят с продажи",
                        _ => "Неизвестный статус"
                    }
                };
            })
            .ToList();


            return Ok(new
            {
                Products = productViewModels,
                TotalItems = totalItems,
                TotalPages = pageSize.HasValue ? (int)Math.Ceiling((double)totalItems / pageSize.Value) : (int?)null,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }
    }
}
