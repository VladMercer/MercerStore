using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Models.Invoices;
using MercerStore.Web.Application.Requests.Invoices;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;

namespace MercerStore.Web.Application.Interfaces.Repositories;

public interface IInvoiceRepository
{
    Task AddInvoiceItems(IEnumerable<InvoiceItem> invoiceItems, CancellationToken ct);
    Task AddInvoice(Invoice invoice, CancellationToken ct);
    Task<Invoice?> GetInvoiceById(int invoiceId, CancellationToken ct);
    Task<Invoice?> GetInvoiceByManagerId(string? managerId, CancellationToken ct);
    Task UpdateInvoice(Invoice invoice, CancellationToken ct);

    Task<(IEnumerable<Invoice>, int totalItems)> GetFilteredInvoices(InvoiceFilterRequest request,
        CancellationToken ct);

    Task UpdateInvoiceItems(IEnumerable<InvoiceItemViewModel> invoiceItems, CancellationToken ct);
    Task<InvoiceMetricDto> GetInvoiceMetric(CancellationToken ct);
    Task AddInvoiceItem(InvoiceItem invoiceItem, CancellationToken ct);
}
