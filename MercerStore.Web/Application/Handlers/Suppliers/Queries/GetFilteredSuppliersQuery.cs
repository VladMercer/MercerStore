using MediatR;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.SupplierDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Suppliers;

namespace MercerStore.Web.Application.Handlers.Suppliers.Queries
{
    public record GetFilteredSuppliersQuery(SupplierFilterRequest Request) : IRequest<PaginatedResultDto<AdminSupplierDto>>;
    public class GetFilterredSuppliersHandler : IRequestHandler<GetFilteredSuppliersQuery, PaginatedResultDto<AdminSupplierDto>>
    {
        private readonly ISupplierService _supplierService;
        private readonly IRedisCacheService _redisCacheService;

        public GetFilterredSuppliersHandler(ISupplierService supplierService, IRedisCacheService redisCacheService)
        {
            _supplierService = supplierService;
            _redisCacheService = redisCacheService;
        }

        public async Task<PaginatedResultDto<AdminSupplierDto>> Handle(GetFilteredSuppliersQuery query, CancellationToken cancellationToken)
        {
            var request = query.Request;
            bool isDefaultQuery =
                request.PageNumber == 1 &&
                request.PageSize == 30 &&
                request.Query == "";

            string cacheKey = $"suppliers:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => _supplierService.GetFilteredSuppliersWithoutCache(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
    }
}
