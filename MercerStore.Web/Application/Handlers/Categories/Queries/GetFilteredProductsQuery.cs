using MediatR;
using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Categories;

namespace MercerStore.Web.Application.Handlers.Categories.Queries
{
    public record GetFilteredProductsQuery(CateroryFilterRequest Request) : IRequest<PaginatedResultDto<ProductDto>>;
    public class GetFilteredProductsHandler : IRequestHandler<GetFilteredProductsQuery, PaginatedResultDto<ProductDto>>
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ICategoryService _categoryService;

        public GetFilteredProductsHandler(IRedisCacheService redisCacheService, ICategoryService categoryService)
        {
            _redisCacheService = redisCacheService;
            _categoryService = categoryService;
        }

        public async Task<PaginatedResultDto<ProductDto>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
        {
            var query = request.Request;

            var priceRange = await _categoryService.GetPriceRange(query.CategoryId);

            bool isDefaultQuery =
                query.PageNumber == 1 &&
                query.PageSize == 9 &&
                string.IsNullOrEmpty(query.SortOrder) &&
                query.MinPrice == priceRange.MinPrice &&
                query.MaxPrice == priceRange.MaxPrice;

            string cacheKey = $"products:{query.CategoryId}:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => _categoryService.GetFilteredProductsWithoutCache(query),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
    }
}
