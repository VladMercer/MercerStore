using MediatR;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Dtos.Supplier;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Suppliers;

namespace MercerStore.Web.Application.Handlers.Suppliers.Queries;

public record GetFilteredSuppliersQuery(SupplierFilterRequest Request) : IRequest<PaginatedResultDto<AdminSupplierDto>>;

public class
    GetFilteredSuppliersHandler : IRequestHandler<GetFilteredSuppliersQuery, PaginatedResultDto<AdminSupplierDto>>
{
    private readonly IRedisCacheService _redisCacheService;
    private readonly ISupplierService _supplierService;

    public GetFilteredSuppliersHandler(ISupplierService supplierService, IRedisCacheService redisCacheService)
    {
        _supplierService = supplierService;
        _redisCacheService = redisCacheService;
    }

    public async Task<PaginatedResultDto<AdminSupplierDto>> Handle(GetFilteredSuppliersQuery query,
        CancellationToken ct)
    {
        var request = query.Request;
        var isDefaultQuery =
            request is { PageNumber: 1, PageSize: 30, Query: "" };

        const string cacheKey = "suppliers:page1";

        return await _redisCacheService.TryGetOrSetCacheAsync(
            cacheKey,
            () => _supplierService.GetFilteredSuppliersWithoutCache(request, ct),
            isDefaultQuery,
            TimeSpan.FromMinutes(10)
        );
    }
}
