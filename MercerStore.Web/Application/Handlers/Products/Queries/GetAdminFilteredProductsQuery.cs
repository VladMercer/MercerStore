using MediatR;
using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Products;

namespace MercerStore.Web.Application.Handlers.Products.Queries
{
    public record GetAdminFilteredProductsQuery(ProductFilterRequest Request) : IRequest<PaginatedResultDto<AdminProductDto>>;
    public class GetAdminFilteredProductsHandler : IRequestHandler<GetAdminFilteredProductsQuery, PaginatedResultDto<AdminProductDto>>
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly IProductService _productService;

        public GetAdminFilteredProductsHandler(IRedisCacheService redisCacheService, IProductService productService)
        {
            _redisCacheService = redisCacheService;
            _productService = productService;
        }

        public async Task<PaginatedResultDto<AdminProductDto>> Handle(GetAdminFilteredProductsQuery query, CancellationToken cancellationToken)
        {
            var request = query.Request;

            bool isDefaultQuery =
               request.CategoryId == null &&
               request.PageNumber == 1 &&
               request.PageSize == 30 &&
               !request.SortOrder.HasValue &&
               !request.Filter.HasValue;
            string cacheKey = $"products:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => _productService.GetFilteredProductsWithoutCache(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
    }
}
