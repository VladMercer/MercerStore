using MediatR;
using MercerStore.Web.Application.Dtos.Invoice;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Invoices;

namespace MercerStore.Web.Application.Handlers.Invoices.Queries;

public record GetFilteredInvoicesQuery(InvoiceFilterRequest Request) : IRequest<PaginatedResultDto<InvoiceDto>>;

public class GetFilteredInvoicesHandler : IRequestHandler<GetFilteredInvoicesQuery, PaginatedResultDto<InvoiceDto>>
{
    private readonly IInvoiceService _invoiceService;
    private readonly IRedisCacheService _redisCacheService;

    public GetFilteredInvoicesHandler(IInvoiceService invoiceService, IRedisCacheService redisCacheService)
    {
        _invoiceService = invoiceService;
        _redisCacheService = redisCacheService;
    }

    public async Task<PaginatedResultDto<InvoiceDto>> Handle(GetFilteredInvoicesQuery query,
        CancellationToken ct)
    {
        var request = query.Request;

        var isDefaultQuery =
            request is { PageNumber: 1, PageSize: 30, SortOrder: null, Filter: null, Query: "" };

        const string cacheKey = "invoices:page1";

        return await _redisCacheService.TryGetOrSetCacheAsync(
            cacheKey,
            () => _invoiceService.GetFilteredInvoicesWithoutCache(request, ct),
            isDefaultQuery,
            TimeSpan.FromMinutes(10)
        );
    }
}
