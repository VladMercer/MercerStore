using MercerStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/metrics")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewProductRepository _reviewProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public MetricsController(IProductRepository productRepository, IOrderRepository orderRepository, IReviewProductRepository reviewProductRepository, IUserActivityRepository userActivityRepository, ISupplierRepository supplierRepository, IInvoiceRepository invoiceRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _reviewProductRepository = reviewProductRepository;
            _supplierRepository = supplierRepository;
            _invoiceRepository = invoiceRepository;
            _userRepository = userRepository;
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {

            var salesMetric = await _orderRepository.GetSalesMetric();
            var reviewMetric = await _reviewProductRepository.GetReviewMetric();
            var userMetric = await _userRepository.GetUserMetric();
            var supplierMetric = await _supplierRepository.GetSupplierMetric();
            var invoiceMetric = await _invoiceRepository.GetInvoiceMetric();

            var metrics = new
            {
                Sales = salesMetric,
                Reviews = reviewMetric,
                Users = userMetric,
                Suppliers = supplierMetric,
                Invoices = invoiceMetric

            };

            return Ok(metrics);
        }
    }
}
