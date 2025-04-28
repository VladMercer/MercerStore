using MercerStore.Web.Application.Dtos.Invoice;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Models.Invoices;
using MercerStore.Web.Application.Requests.Invoices;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IInvoiceService
{
    Task<PaginatedResultDto<InvoiceDto>> GetFilteredInvoicesWithoutCache(InvoiceFilterRequest request,
        CancellationToken ct);

    Task<CreateInvoiceViewModel> GetCreateInvoiceViewModel(int supplierId, string managerId, CancellationToken ct);
    Task<Result<Invoice>> AddInvoiceItem(CreateInvoiceViewModel createInvoiceViewModel, CancellationToken ct);
    Task<Result<Invoice>> CloseInvoice(int invoiceId, string notes, CancellationToken ct);
    Task<UpdateInvoiceViewModel> GetUpdateInvoiceViewModel(int invoiceId, CancellationToken ct);
    Task<Invoice> UpdateInvoice(UpdateInvoiceViewModel updateInvoiceViewModel, CancellationToken ct);
}