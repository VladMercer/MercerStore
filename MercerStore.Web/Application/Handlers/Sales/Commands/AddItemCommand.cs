using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Application.Requests.Sales;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Sales.Commands;

public record AddItemCommand(SaleRequest SaleRequest) :
    LoggableRequest<Result<Unit>>("Manager add item in Offline sale", "OfflineSaleItem");

public class AddItemHandler : IRequestHandler<AddItemCommand, Result<Unit>>
{
    private readonly ISaleService _saleService;

    public AddItemHandler(ISaleService saleService)
    {
        _saleService = saleService;
    }

    public async Task<Result<Unit>> Handle(AddItemCommand request, CancellationToken ct)
    {
        var result = await _saleService.AddItem(request.SaleRequest, ct);

        if (!result.IsSuccess) return Result<Unit>.Failure(result.ErrorMessage);

        var logDetails = new
        {
            OfflineSaleId = request.SaleRequest.SaleId,
            request.SaleRequest.ProductId,
            request.SaleRequest.Sku,
            request.SaleRequest.Quantity
        };

        request.EntityId = request.SaleRequest.SaleId;
        request.Details = logDetails;

        return Result<Unit>.Success(Unit.Value);
    }
}
