using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Invoices.Command;

public record AddInvoiceItemCommand(CreateInvoiceViewModel CreateInvoiceViewModel) :
    LoggableRequest<Result<Unit>>("Manager add item in invoice", "invoiceItem");

public class AddInvoiceItemHandler : IRequestHandler<AddInvoiceItemCommand, Result<Unit>>
{
    private readonly IInvoiceService _invoiceService;

    public AddInvoiceItemHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<Result<Unit>> Handle(AddInvoiceItemCommand request, CancellationToken ct)
    {
        var result = await _invoiceService.AddInvoiceItem(request.CreateInvoiceViewModel, ct);

        request.EntityId = request.CreateInvoiceViewModel.ProductId;

        if (!result.IsSuccess)
        {
            request.Details = new { result.ErrorMessage };
            return Result<Unit>.Failure(result.ErrorMessage);
        }

        request.Details = new
        {
            result.Data
        };

        return Result<Unit>.Success(Unit.Value);
    }
}