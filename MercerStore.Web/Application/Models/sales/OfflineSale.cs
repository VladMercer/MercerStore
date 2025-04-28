namespace MercerStore.Web.Application.Models.sales;

public class OfflineSale
{
    public int Id { get; set; }
    public string ManagerId { get; set; }
    public bool IsClosed { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public IList<OfflineSaleItem>? Items { get; set; } = [];
}