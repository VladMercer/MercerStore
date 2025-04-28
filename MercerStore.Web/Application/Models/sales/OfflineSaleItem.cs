namespace MercerStore.Web.Application.Models.sales;

public class OfflineSaleItem
{
    public int Id { get; set; }
    public int OfflineSaleId { get; set; }
    public int? ProductId { get; set; }
    public string? SKU { get; set; }
    public decimal ItemPrice { get; set; }
    public int Quantity { get; set; }

    public OfflineSale? Sale { get; set; }
}