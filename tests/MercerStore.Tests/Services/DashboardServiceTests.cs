using Moq;
using MercerStore.Web.Application.Services;
using MercerStore.Web.Application.Interfaces.Repositories;

namespace MercerStore.Tests.Services
{
    public class DashboardServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly DashboardService _dashboardService;

        public DashboardServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _dashboardService = new DashboardService(_mockOrderRepository.Object);
        }

        [Fact]
        public async Task GetDashboardViewMetric_ShouldReturnCorrectMetrics_WhenDataIsValid()
        {
            // Arrange
            var expectedRevenue = 5000m;
            var expectedOrderCount = 150;

            _mockOrderRepository.Setup(repo => repo.GetRevenue()).ReturnsAsync(expectedRevenue);
            _mockOrderRepository.Setup(repo => repo.GetOrdersCount()).ReturnsAsync(expectedOrderCount);

            // Act
            var result = await _dashboardService.GetDashboardViewMetric();

            // Assert
            Assert.Equal(expectedRevenue, result.Revenue);
            Assert.Equal(expectedOrderCount, result.OrdersCount);

            _mockOrderRepository.Verify(repo => repo.GetRevenue(), Times.Once);
            _mockOrderRepository.Verify(repo => repo.GetOrdersCount(), Times.Once);
        }

        [Fact]
        public async Task GetDashboardViewMetric_ShouldReturnZeroMetrics_WhenNoData()
        {
            // Arrange
            var expectedRevenue = 0m;
            var expectedOrderCount = 0;

            _mockOrderRepository.Setup(repo => repo.GetRevenue()).ReturnsAsync(expectedRevenue);
            _mockOrderRepository.Setup(repo => repo.GetOrdersCount()).ReturnsAsync(expectedOrderCount);

            // Act
            var result = await _dashboardService.GetDashboardViewMetric();

            // Assert
            Assert.Equal(expectedRevenue, result.Revenue);
            Assert.Equal(expectedOrderCount, result.OrdersCount);
        }
    }
}