using MediatR;
using MercerStore.Web.Application.Dtos.Product;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Categories;

namespace MercerStore.Web.Application.Handlers.Categories.Queries;

public record GetFilteredProductsQuery(CategoryFilterRequest Request) : IRequest<PaginatedResultDto<ProductDto>>;

public class GetFilteredProductsHandler : IRequestHandler<GetFilteredProductsQuery, PaginatedResultDto<ProductDto>>
{
    private readonly ICategoryService _categoryService;
    private readonly IRedisCacheService _redisCacheService;

    public GetFilteredProductsHandler(IRedisCacheService redisCacheService, ICategoryService categoryService)
    {
        _redisCacheService = redisCacheService;
        _categoryService = categoryService;
    }

    public async Task<PaginatedResultDto<ProductDto>> Handle(GetFilteredProductsQuery request,
        CancellationToken ct)
    {
        var query = request.Request;

        var priceRange = await _categoryService.GetPriceRange(query.CategoryId, ct);

        var isDefaultQuery =
            query is { PageNumber: 1, PageSize: 9 } &&
            string.IsNullOrEmpty(query.SortOrder) &&
            query.MinPrice == priceRange.MinPrice &&
            query.MaxPrice == priceRange.MaxPrice;

        var cacheKey = $"products:{query.CategoryId}:page1";

        return await _redisCacheService.TryGetOrSetCacheAsync(
            cacheKey,
            () => _categoryService.GetFilteredProductsWithoutCache(query, ct),
            isDefaultQuery,
            TimeSpan.FromMinutes(10)
        );
    }
}