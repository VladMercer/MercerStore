using MercerStore.Web.Application.Dtos.InvoiceDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Infrastructure.Data.Enum.Invoice;
using MercerStore.Web.Infrastructure.Data.Enum;
using MercerStore.Web.Application.Requests.Invoices;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task<PaginatedResultDto<InvoiceDto>> GetFilteredInvoices(InvoiceFilterRequest request);
        Task<CreateInvoiceViewModel> AddInvoice(int supplierId);
        Task<Result<AddInvoiceItemResultViewModel>> AddInvoiceItem(CreateInvoiceViewModel createInvoiceViewModel);
        Task<Result<int>> CloseInvoice(int invoiceId, string notes);
        Task<UpdateInvoiceViewModel> GetUpdateInvoiceViewModel(int invoiceId);
        Task<int> UpdateInvoice(UpdateInvoiceViewModel updateInvoiceViewModel);


    }
}
