using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Sales.Commands;

public record CloseSaleCommand(int SaleId) :
    LoggableRequest<Result<Unit>>("Manager close offline sale", "OfflineSale");

public class CloseSaleHandler : IRequestHandler<CloseSaleCommand, Result<Unit>>
{
    private readonly ISaleService _saleService;

    public CloseSaleHandler(ISaleService saleService)
    {
        _saleService = saleService;
    }

    public async Task<Result<Unit>> Handle(CloseSaleCommand request, CancellationToken ct)
    {
        var result = await _saleService.CloseSale(request.SaleId, ct);
        if (!result.IsSuccess) return Result<Unit>.Failure(result.ErrorMessage);

        request.EntityId = request.SaleId;

        return Result<Unit>.Success(Unit.Value);
    }
}