using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;

namespace MercerStore.Web.Application.Handlers.Invoices.Command;

public record UpdateInvoiceCommand(UpdateInvoiceViewModel UpdateInvoiceViewModel) :
    LoggableRequest<Unit>("Manager update invoice", "invoice");

public class UpdateInvoiceHandler : IRequestHandler<UpdateInvoiceCommand, Unit>
{
    private readonly IDateTimeConverter _dateTimeConverter;
    private readonly IInvoiceService _invoiceService;

    public UpdateInvoiceHandler(IInvoiceService invoiceService, IDateTimeConverter dateTimeConverter)
    {
        _invoiceService = invoiceService;
        _dateTimeConverter = dateTimeConverter;
    }

    public async Task<Unit> Handle(UpdateInvoiceCommand request, CancellationToken ct)
    {
        if (request.UpdateInvoiceViewModel.PaymentDate.HasValue)
            request.UpdateInvoiceViewModel.PaymentDate =
                _dateTimeConverter.ConvertUserTimeToUtc(request.UpdateInvoiceViewModel.PaymentDate.Value);

        var result = await _invoiceService.UpdateInvoice(request.UpdateInvoiceViewModel, ct);

        request.EntityId = result.Id;
        request.Details = new { result };

        return Unit.Value;
    }
}