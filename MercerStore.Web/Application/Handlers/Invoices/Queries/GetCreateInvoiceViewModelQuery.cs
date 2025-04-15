using MediatR;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;

namespace MercerStore.Web.Application.Handlers.Invoices.Queries
{
    public record GetCreateInvoiceViewModelQuery(int SupplierId) : IRequest<CreateInvoiceViewModel>;
    public class GetCreateInvoiceViewModelHandler : IRequestHandler<GetCreateInvoiceViewModelQuery, CreateInvoiceViewModel>
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IUserIdentifierService _userIdentifierService;
        public GetCreateInvoiceViewModelHandler(IInvoiceService invoiceService, IUserIdentifierService userIdentifierService)
        {
            _invoiceService = invoiceService;
            _userIdentifierService = userIdentifierService;
        }

        public async Task<CreateInvoiceViewModel> Handle(GetCreateInvoiceViewModelQuery request, CancellationToken cancellationToken)
        {
            var managerId = _userIdentifierService.GetCurrentIdentifier();
            return await _invoiceService.GetCreateInvoiceViewModel(request.SupplierId, managerId);
        }
    }
}
