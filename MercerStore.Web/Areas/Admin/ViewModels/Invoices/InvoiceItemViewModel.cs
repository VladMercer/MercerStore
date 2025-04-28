namespace MercerStore.Web.Areas.Admin.ViewModels.Invoices;

public class InvoiceItemViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
}