using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Models.sales;
using MercerStore.Web.Application.Requests.Sales;
using MercerStore.Web.Application.Services;
using Moq;

namespace MercerStore.Tests.Services
{
    public class SaleServiceTests
    {
        private readonly Mock<ILogService> _logServiceMock;
        private readonly Mock<IRequestContextService> _requestContextServiceMock;
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IUserIdentifierService> _userIdentifierServiceMock;
        private readonly SaleService _saleService;

        public SaleServiceTests()
        {
            _logServiceMock = new Mock<ILogService>();
            _requestContextServiceMock = new Mock<IRequestContextService>();
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _userIdentifierServiceMock = new Mock<IUserIdentifierService>();

            _saleService = new SaleService(
                _logServiceMock.Object,
                _requestContextServiceMock.Object,
                _saleRepositoryMock.Object,
                _productRepositoryMock.Object,
                _userIdentifierServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateOfflineSale_ShouldReturnExistingSale_IfSaleExists()
        {
            // Arrange
            var managerId = "manager-123";
            var existingSale = new OfflineSale { Id = 1, ManagerId = managerId };

            _userIdentifierServiceMock.Setup(s => s.GetCurrentIdentifier()).Returns(managerId);
            _saleRepositoryMock.Setup(r => r.GetSaleByManagerId(managerId)).ReturnsAsync(existingSale);

            // Act
            var result = await _saleService.CreateOfflineSale();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingSale.Id, result.Id);
        }

        [Fact]
        public async Task CreateOfflineSale_ShouldCreateNewSale_IfNoneExists()
        {
            // Arrange
            var managerId = "manager-123";
            OfflineSale? existingSale = null;

            _userIdentifierServiceMock.Setup(s => s.GetCurrentIdentifier()).Returns(managerId);
            _saleRepositoryMock.Setup(r => r.GetSaleByManagerId(managerId)).ReturnsAsync(existingSale);
            _saleRepositoryMock.Setup(r => r.AddOfflineSales(It.IsAny<OfflineSale>()))
                .Callback<OfflineSale>(sale => sale.Id = 2)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _saleService.CreateOfflineSale();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal(managerId, result.ManagerId);
        }

        [Fact]
        public async Task AddItem_ShouldReturnFailure_IfSaleNotFoundOrClosed()
        {
            // Arrange
            var request = new SaleRequest(1, 15, null, 1);

            _saleRepositoryMock.Setup(r => r.GetSaleById(1)).ReturnsAsync((OfflineSale?)null);

            // Act
            var result = await _saleService.AddItem(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Продажа не найдена", result.ErrorMessage);
        }

        [Fact]
        public async Task AddItem_ShouldReturnFailure_IfProductNotFound()
        {
            // Arrange
            var sale = new OfflineSale { Id = 1, IsClosed = false };
            var request = new SaleRequest(1, 15, null, 1);

            _saleRepositoryMock.Setup(r => r.GetSaleById(1)).ReturnsAsync(sale);
            _productRepositoryMock.Setup(p => p.GetProductByIdAsync(100)).ReturnsAsync((Product?)null);

            // Act
            var result = await _saleService.AddItem(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Товар не найден", result.ErrorMessage);
        }

        [Fact]
        public async Task AddItem_ShouldAddItemToSale()
        {
            // Arrange
            var sale = new OfflineSale { Id = 1, IsClosed = false };
            var product = new Product { Id = 100, ProductPricing = new ProductPricing { OriginalPrice = 500 } };
            var request = new SaleRequest(1, 100, null, 2);

            _saleRepositoryMock.Setup(r => r.GetSaleById(1)).ReturnsAsync(sale);
            _productRepositoryMock.Setup(p => p.GetProductByIdAsync(100)).ReturnsAsync(product);
            _saleRepositoryMock.Setup(r => r.AddOfflineSaleItems(It.IsAny<OfflineSaleItem>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _saleService.AddItem(request);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CloseSale_ShouldReturnFailure_IfSaleNotFoundOrClosed()
        {
            // Arrange
            _saleRepositoryMock.Setup(r => r.GetSaleById(1)).ReturnsAsync((OfflineSale?)null);

            // Act
            var result = await _saleService.CloseSale(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Продажа не найдена", result.ErrorMessage);
        }

        [Fact]
        public async Task CloseSale_ShouldUpdateStockAndCloseSale()
        {
            // Arrange
            var sale = new OfflineSale
            {
                Id = 1,
                IsClosed = false,
                Items = new List<OfflineSaleItem>
                {
                    new OfflineSaleItem { ProductId = 100, Quantity = 2, ItemPrice = 500 }
                }
            };

            _saleRepositoryMock.Setup(r => r.GetSaleById(1)).ReturnsAsync(sale);
            _saleRepositoryMock.Setup(r => r.UpdateSale(sale)).Returns(Task.CompletedTask);
            _productRepositoryMock.Setup(p => p.DecreaseProductStock(100, null, 2)).Returns(Task.CompletedTask);

            // Act
            var result = await _saleService.CloseSale(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(sale.IsClosed);
            Assert.Equal(1000, sale.TotalPrice);
        }

        [Fact]
        public async Task GetSummarySale_ShouldReturnFailure_IfSaleNotFound()
        {
            // Arrange
            _saleRepositoryMock.Setup(r => r.GetSaleById(1)).ReturnsAsync((OfflineSale?)null);

            // Act
            var result = await _saleService.GetSummarySale(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Продажа не найдена", result.ErrorMessage);
        }

        [Fact]
        public async Task GetSummarySale_ShouldReturnSaleDetails()
        {
            // Arrange
            var sale = new OfflineSale { Id = 1 };
            _saleRepositoryMock.Setup(r => r.GetSaleById(1)).ReturnsAsync(sale);

            // Act
            var result = await _saleService.GetSummarySale(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(sale, result.Data);
        }
    }
}