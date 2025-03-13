namespace MercerStore.Web.Application.Dtos.MetricDto
{
    public class MetricDto
    {
        public SalesMetricDto Sales { get; set; }
        public ReviewMetricDto Reviews { get; set; }
        public UserMetricDto Users { get; set; }
        public SupplierMetricDto Suppliers { get; set; }
        public InvoiceMetricDto Invoices { get; set; }
    }
}
