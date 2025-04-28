using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Invoices.Command;

public record CloseInvoiceCommand(int InvoiceId, string Notes) :
    LoggableRequest<Result<Unit>>("Manager close invoice", "invoice");

public class CloseInvoiceHandler : IRequestHandler<CloseInvoiceCommand, Result<Unit>>
{
    private readonly IInvoiceService _invoiceService;

    public CloseInvoiceHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<Result<Unit>> Handle(CloseInvoiceCommand request, CancellationToken ct)
    {
        var result = await _invoiceService.CloseInvoice(request.InvoiceId, request.Notes, ct);

        request.EntityId = request.InvoiceId;

        if (!result.IsSuccess)
        {
            request.Details = new { result.ErrorMessage };
            return Result<Unit>.Failure(result.ErrorMessage);
        }

        request.Details = new { result.Data };

        return Result<Unit>.Success(Unit.Value);
    }
}