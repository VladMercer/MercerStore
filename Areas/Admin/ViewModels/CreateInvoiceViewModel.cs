namespace MercerStore.Areas.Admin.ViewModels
{
    public class CreateInvoiceViewModel
    {
        public int InvoiceId { get; set; }
        public int? ProductId { get; set; }
        public string? Sku { get; set; }
        public int Quantity { get; set; }
        public int ProductPrice { get; set; }
        public string? Notes { get; set; }
        public List<InvoiceProductSelectionViewModel> AvailableProducts { get; set; } = [];
    }
}
