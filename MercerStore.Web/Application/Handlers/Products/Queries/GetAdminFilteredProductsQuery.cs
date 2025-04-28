using MediatR;
using MercerStore.Web.Application.Dtos.Product;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Products;

namespace MercerStore.Web.Application.Handlers.Products.Queries;

public record GetAdminFilteredProductsQuery(ProductFilterRequest Request)
    : IRequest<PaginatedResultDto<AdminProductDto>>;

public class
    GetAdminFilteredProductsHandler : IRequestHandler<GetAdminFilteredProductsQuery,
    PaginatedResultDto<AdminProductDto>>
{
    private readonly IProductService _productService;
    private readonly IRedisCacheService _redisCacheService;

    public GetAdminFilteredProductsHandler(IRedisCacheService redisCacheService, IProductService productService)
    {
        _redisCacheService = redisCacheService;
        _productService = productService;
    }

    public async Task<PaginatedResultDto<AdminProductDto>> Handle(GetAdminFilteredProductsQuery query,
        CancellationToken ct)
    {
        var request = query.Request;

        var isDefaultQuery =
            request is { CategoryId: null, PageNumber: 1, PageSize: 30, SortOrder: null, Filter: null };
        const string cacheKey = "products:page1";

        return await _redisCacheService.TryGetOrSetCacheAsync(
            cacheKey,
            () => _productService.GetFilteredProductsWithoutCache(request, ct),
            isDefaultQuery,
            TimeSpan.FromMinutes(10)
        );
    }
}