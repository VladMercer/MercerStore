namespace MercerStore.Web.Application.Dtos.MetricDto
{
    public class InvoiceMetricDto
    {
        public decimal Daily { get; set; }
        public decimal Weekly { get; set; }
        public decimal Monthly { get; set; }
        public decimal Yearly { get; set; }

        public TotalInvoicesDto TotalInvoices { get; set; }
        public decimal AverageInvoiceValue { get; set; }
        public List<TopProductDto> TopProducts { get; set; }
    }
}
