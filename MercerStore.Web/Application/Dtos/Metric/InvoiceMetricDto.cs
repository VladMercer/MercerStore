namespace MercerStore.Web.Application.Dtos.Metric;

public class InvoiceMetricDto
{
    public decimal Daily { get; set; }
    public decimal Weekly { get; set; }
    public decimal Monthly { get; set; }
    public decimal Yearly { get; set; }

    public TotalInvoicesDto? TotalInvoices { get; set; }
    public decimal AverageInvoiceValue { get; set; }
    public IList<TopProductDto>? TopProducts { get; set; }
}
