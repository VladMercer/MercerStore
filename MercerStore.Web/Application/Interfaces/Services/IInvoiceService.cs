using MercerStore.Web.Application.Dtos.InvoiceDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Infrastructure.Data.Enum.Invoice;
using MercerStore.Web.Infrastructure.Data.Enum;
using MercerStore.Web.Application.Requests.Invoices;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Infrastructure.Helpers;
using MercerStore.Web.Application.Models.Invoice;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task<PaginatedResultDto<InvoiceDto>> GetFilteredInvoicesWithoutCache(InvoiceFilterRequest request);
        Task<CreateInvoiceViewModel> GetCreateInvoiceViewModel(int supplierId, string managerId);
        Task<Result<Invoice>> AddInvoiceItem(CreateInvoiceViewModel createInvoiceViewModel);
        Task<Result<Invoice>> CloseInvoice(int invoiceId, string notes);
        Task<UpdateInvoiceViewModel> GetUpdateInvoiceViewModel(int invoiceId);
        Task<Invoice> UpdateInvoice(UpdateInvoiceViewModel updateInvoiceViewModel);


    }
}
