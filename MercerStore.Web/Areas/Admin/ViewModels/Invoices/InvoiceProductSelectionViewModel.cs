namespace MercerStore.Web.Areas.Admin.ViewModels.Invoices
{
    public class InvoiceProductSelectionViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public int Quantity { get; set; } = 1;
        public int PurchasePrice { get; set; }
    }
}
