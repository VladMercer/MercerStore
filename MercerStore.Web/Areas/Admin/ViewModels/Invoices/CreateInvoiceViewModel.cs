namespace MercerStore.Web.Areas.Admin.ViewModels.Invoices;

public class CreateInvoiceViewModel
{
    public int InvoiceId { get; set; }
    public int? ProductId { get; set; }
    public int SupplierId { get; set; }
    public string? Sku { get; set; }
    public int Quantity { get; set; }
    public int ProductPrice { get; set; }
    public string? Notes { get; set; }
    public IList<InvoiceProductSelectionViewModel> AvailableProducts { get; set; } = [];
}