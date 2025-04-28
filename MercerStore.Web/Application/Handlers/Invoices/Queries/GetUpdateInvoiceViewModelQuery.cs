using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;

namespace MercerStore.Web.Application.Handlers.Invoices.Queries;

public record GetUpdateInvoiceViewModelQuery(int InvoiceId) : IRequest<UpdateInvoiceViewModel>;

public class GetUpdateInvoiceViewModelHandler : IRequestHandler<GetUpdateInvoiceViewModelQuery, UpdateInvoiceViewModel>
{
    private readonly IDateTimeConverter _dateTimeConverter;
    private readonly IInvoiceService _invoiceService;

    public GetUpdateInvoiceViewModelHandler(IInvoiceService invoiceService, IDateTimeConverter dateTimeConverter)
    {
        _invoiceService = invoiceService;
        _dateTimeConverter = dateTimeConverter;
    }

    public async Task<UpdateInvoiceViewModel> Handle(GetUpdateInvoiceViewModelQuery request,
        CancellationToken ct)
    {
        var updateInvoiceViewModel = await _invoiceService.GetUpdateInvoiceViewModel(request.InvoiceId, ct);

        if (updateInvoiceViewModel.EditDate.HasValue)
            updateInvoiceViewModel.EditDate =
                _dateTimeConverter.ConvertUtcToUserTime(updateInvoiceViewModel.EditDate.Value);

        if (updateInvoiceViewModel.PaymentDate.HasValue)
            updateInvoiceViewModel.PaymentDate =
                _dateTimeConverter.ConvertUtcToUserTime(updateInvoiceViewModel.PaymentDate.Value);

        updateInvoiceViewModel.DateReceived =
            _dateTimeConverter.ConvertUtcToUserTime(updateInvoiceViewModel.DateReceived);

        return updateInvoiceViewModel;
    }
}