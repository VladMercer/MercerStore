namespace MercerStore.Web.Application.Dtos.MetricDto
{
    public class SalesMetricDto
    {
        public decimal Daily { get; set; }
        public decimal Weekly { get; set; }
        public decimal Monthly { get; set; }
        public decimal Yearly { get; set; }

        public TotalOrderDto TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<TopProductDto> TopProducts { get; set; }
    }
}
