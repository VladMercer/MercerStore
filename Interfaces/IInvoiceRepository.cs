using MercerStore.Data.Enum.Invoice;
using MercerStore.Data.Enum;
using MercerStore.Dtos.InvoiceDto;
using MercerStore.Models.Invoice;
using MercerStore.Areas.Admin.ViewModels;

namespace MercerStore.Interfaces
{
    public interface IInvoiceRepository
    {
        Task AddInvoiceItems(InvoiceItem invoiceItem);
        Task AddInvoice(Invoice invoice);
        Task<Invoice?> GetInvoiceById(int invoiceId);
        Task<Invoice?> GetInvoiceByManagerId(string? managerId);
        Task UpdateInvoice(Invoice invoice);
        Task<(IEnumerable<InvoiceDto>, int totalItems)> GetFilteredInvoices(
             int pageNumber,
             int pageSize,
             InvoiceSortOrder? sortOrder,
             TimePeriod? timePeriod,
             InvoiceFilter? filter,
             string? query);
        Task UpdateInvoiceItems(List<InvoiceItemViewModel> invoiceItems);
        Task<object> GetInvoiceMetric();

    }
}
