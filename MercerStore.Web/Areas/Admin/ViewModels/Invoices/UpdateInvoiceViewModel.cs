using MercerStore.Web.Infrastructure.Data.Enum.Invoice;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.ViewModels.Invoices;

public class UpdateInvoiceViewModel
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public string? CompanyName { get; set; }
    public string? ManagerId { get; set; }
    public DateTime DateReceived { get; set; }
    public decimal TotalAmount { get; set; }
    public InvoiceStatus Status { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? EditDate { get; set; }
    public string? Notes { get; set; }
    public IList<InvoiceItemViewModel?> InvoiceItems { get; set; } = [];
}