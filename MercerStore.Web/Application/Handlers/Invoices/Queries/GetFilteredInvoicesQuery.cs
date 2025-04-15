using MediatR;
using MercerStore.Web.Application.Dtos.InvoiceDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Requests.Invoices;

namespace MercerStore.Web.Application.Handlers.Invoices.Queries
{
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

        public async Task<PaginatedResultDto<InvoiceDto>> Handle(GetFilteredInvoicesQuery query, CancellationToken cancellationToken)
        {
            var request = query.Request;

            bool isDefaultQuery =
                request.PageNumber == 1 &&
                request.PageSize == 30 &&
                !request.SortOrder.HasValue &&
                !request.Filter.HasValue &&
                request.Query == "";

            string cacheKey = $"invoices:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => _invoiceService.GetFilteredInvoicesWithoutCache(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
    }
}
