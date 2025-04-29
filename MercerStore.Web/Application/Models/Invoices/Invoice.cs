using MercerStore.Web.Infrastructure.Data.Enum.Invoice;

namespace MercerStore.Web.Application.Models.Invoices;

public class Invoice
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public string ManagerId { get; set; }
    public DateTime DateReceived { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public IList<InvoiceItem> InvoiceItems { get; set; } = [];
    public string? Notes { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;
    public DateTime? PaymentDate { get; set; }
    public DateTime? EditDate { get; set; }
}
