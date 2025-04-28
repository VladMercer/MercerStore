using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.sales;

namespace MercerStore.Web.Application.Handlers.Sales.Queries;

public record CreateOfflineSaleQuery : IRequest<OfflineSale>;

public class CreateOfflineSaleHandler : IRequestHandler<CreateOfflineSaleQuery, OfflineSale>
{
    private readonly ISaleService _saleService;
    private readonly IUserIdentifierService _userIdentifierService;

    public CreateOfflineSaleHandler(ISaleService saleService, IUserIdentifierService userIdentifierService)
    {
        _saleService = saleService;
        _userIdentifierService = userIdentifierService;
    }

    public async Task<OfflineSale> Handle(CreateOfflineSaleQuery request, CancellationToken ct)
    {
        var managerId = _userIdentifierService.GetCurrentIdentifier();
        return await _saleService.CreateOfflineSale(managerId, ct);
    }
}