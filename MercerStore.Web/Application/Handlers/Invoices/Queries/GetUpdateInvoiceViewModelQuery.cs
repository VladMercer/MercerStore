using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MediatR;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Invoices.Queries
{
    public record GetUpdateInvoiceViewModelQuery(int InvoiceId) : IRequest<UpdateInvoiceViewModel>;
    public class GetUpdateInvoiceViewModelHandler : IRequestHandler<GetUpdateInvoiceViewModelQuery, UpdateInvoiceViewModel>
    {
        private readonly IInvoiceService _invoiceService;

        public GetUpdateInvoiceViewModelHandler(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public async Task<UpdateInvoiceViewModel> Handle(GetUpdateInvoiceViewModelQuery request, CancellationToken cancellationToken)
        {
            return await _invoiceService.GetUpdateInvoiceViewModel(request.InvoiceId);
        }
    }
}
