using MercerStore.Web.Application.Models.Invoice;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Application.Requests.Invoices;
using MercerStore.Web.Application.Dtos.MetricDto;

namespace MercerStore.Web.Application.Interfaces.Repositories
{
    public interface IInvoiceRepository
    {
        Task AddInvoiceItems(List<InvoiceItem> invoiceItems);
        Task AddInvoice(Invoice invoice);
        Task<Invoice?> GetInvoiceById(int invoiceId);
        Task<Invoice?> GetInvoiceByManagerId(string? managerId);
        Task UpdateInvoice(Invoice invoice);
        Task<(IEnumerable<Invoice>, int totalItems)> GetFilteredInvoices(InvoiceFilterRequest request);
        Task UpdateInvoiceItems(List<InvoiceItemViewModel> invoiceItems);
        Task<InvoiceMetricDto> GetInvoiceMetric();
        Task AddInvoiceItem(InvoiceItem invoiceItem);

    }
}
