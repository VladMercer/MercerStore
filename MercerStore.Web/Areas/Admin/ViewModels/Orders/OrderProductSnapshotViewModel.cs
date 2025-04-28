namespace MercerStore.Web.Areas.Admin.ViewModels.Orders;

public class OrderProductSnapshotViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal PriceAtOrder { get; set; }
    public string ProductName { get; set; }
    public string ProductImageUrl { get; set; }
    public int Quantity { get; set; }
}
