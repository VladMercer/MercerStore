using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Services;

public class MetricService : IMetricService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IReviewProductRepository _reviewProductRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IUserRepository _userRepository;

    public MetricService(
        IOrderRepository orderRepository,
        IReviewProductRepository reviewProductRepository,
        IUserRepository userRepository,
        ISupplierRepository supplierRepository,
        IInvoiceRepository invoiceRepository)
    {
        _orderRepository = orderRepository;
        _reviewProductRepository = reviewProductRepository;
        _userRepository = userRepository;
        _supplierRepository = supplierRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<MetricDto> GetMetrics(CancellationToken ct)
    {
        var salesMetric = await _orderRepository.GetSalesMetric(ct);
        var reviewMetric = await _reviewProductRepository.GetReviewMetric(ct);
        var userMetric = await _userRepository.GetUserMetric(ct);
        var supplierMetric = await _supplierRepository.GetSupplierMetric(ct);
        var invoiceMetric = await _invoiceRepository.GetInvoiceMetric(ct);

        var metrics = new MetricDto
        {
            Sales = salesMetric,
            Reviews = reviewMetric,
            Users = userMetric,
            Suppliers = supplierMetric,
            Invoices = invoiceMetric
        };

        return metrics;
    }
}
