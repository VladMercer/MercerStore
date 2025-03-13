using MercerStore.Web.Application.Dtos.MetricDto;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Services
{
    public class MetricService : IMetricService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewProductRepository _reviewProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public MetricService(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IReviewProductRepository reviewProductRepository,
            IUserRepository userRepository,
            ISupplierRepository supplierRepository,
            IInvoiceRepository invoiceRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _reviewProductRepository = reviewProductRepository;
            _userRepository = userRepository;
            _supplierRepository = supplierRepository;
            _invoiceRepository = invoiceRepository;
        }
        public async Task<MetricDto> GetMetrics()
        {
            var salesMetric = await _orderRepository.GetSalesMetric();
            var reviewMetric = await _reviewProductRepository.GetReviewMetric();
            var userMetric = await _userRepository.GetUserMetric();
            var supplierMetric = await _supplierRepository.GetSupplierMetric();
            var invoiceMetric = await _invoiceRepository.GetInvoiceMetric();

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
}
