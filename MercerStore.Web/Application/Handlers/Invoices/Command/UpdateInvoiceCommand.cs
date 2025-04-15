using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;

namespace MercerStore.Web.Application.Handlers.Invoices.Command
{
    public record UpdateInvoiceCommand(UpdateInvoiceViewModel UpdateInvoiceViewModel) :
        LoggableRequest<Unit>("Manager update invoice", "invoice");
    public class UpdateInvoiceHandler : IRequestHandler<UpdateInvoiceCommand, Unit>
    {
        private readonly IInvoiceService _invoiceService;

        public UpdateInvoiceHandler(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public async Task<Unit> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var result = await _invoiceService.UpdateInvoice(request.UpdateInvoiceViewModel);

            request.EntityId = result.Id;
            request.Details = new { result };

            return Unit.Value;
        }
    }
}
