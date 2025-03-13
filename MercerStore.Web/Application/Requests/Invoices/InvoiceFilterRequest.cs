using MercerStore.Web.Infrastructure.Data.Enum.Invoice;
using MercerStore.Web.Infrastructure.Data.Enum;

namespace MercerStore.Web.Application.Requests.Invoices
{
    public record InvoiceFilterRequest(
        int PageNumber = 1,
        int PageSize = 9,
        InvoiceSortOrder? SortOrder = null,
        TimePeriod? Period = null,
        InvoiceFilter? Filter = null,
        string? Query = null);
}
