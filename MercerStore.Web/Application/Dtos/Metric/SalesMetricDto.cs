namespace MercerStore.Web.Application.Dtos.Metric;

public class SalesMetricDto
{
    public decimal Daily { get; set; }
    public decimal Weekly { get; set; }
    public decimal Monthly { get; set; }
    public decimal Yearly { get; set; }

    public TotalOrderDto TotalOrders { get; set; }
    public decimal AverageOrderValue { get; set; }
    public IList<TopProductDto> TopProducts { get; set; }
}