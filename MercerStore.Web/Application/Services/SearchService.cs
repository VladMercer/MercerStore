using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Dtos.SearchDto;
using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Requests.Search;

namespace MercerStore.Web.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly IProductRepository _productRepository;

        public SearchService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<SearchResultDto> SearchProduct(IEnumerable<ProductIndexDto> productIndexDto, SearchFilterRequest request)
        {
            var productIds = productIndexDto.Select(x => x.Id).ToList();

            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            products = request.SortOrder switch
            {
                "price_asc" => products.OrderBy(p => p.ProductPricing.OriginalPrice).ToList(),
                "price_desc" => products.OrderByDescending(p => p.ProductPricing.OriginalPrice).ToList(),
                "name_asc" => products.OrderBy(p => p.Name).ToList(),
                "name_desc" => products.OrderByDescending(p => p.Name).ToList(),
                _ => products
            };

            var totalItems = products.Count();

            if (request.PageNumber.HasValue && request.PageSize.HasValue)
            {
                products = products
                    .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value).ToList();
            }

            var productIndexDict = productIndexDto.ToDictionary(dto => dto.Id);

            var searchProduct = products
            .Select(p =>
            {
                productIndexDict.TryGetValue(p.Id, out var indexDto);

                return new SearchProductDto
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

            return new SearchResultDto
            {
                Products = searchProduct,
                TotalItems = totalItems,
                TotalPages = request.PageSize.HasValue ? (int)Math.Ceiling((double)totalItems / request.PageSize.Value) : (int?)null,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
