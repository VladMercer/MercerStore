using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.sales;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Sales.Queries;

public record GetSummarySaleQuery(int SaleId) : IRequest<Result<OfflineSale>>;

public class GetSummarySaleHandler : IRequestHandler<GetSummarySaleQuery, Result<OfflineSale>>
{
    private readonly ISaleService _saleService;

    public GetSummarySaleHandler(ISaleService saleService)
    {
        _saleService = saleService;
    }

    public async Task<Result<OfflineSale>> Handle(GetSummarySaleQuery request, CancellationToken ct)
    {
        var result = await _saleService.GetSummarySale(request.SaleId, ct);

        if (!result.IsSuccess) return Result<OfflineSale>.Failure(result.ErrorMessage);

        return result;
    }
}
