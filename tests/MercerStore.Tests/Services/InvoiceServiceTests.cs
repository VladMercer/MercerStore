using MercerStore.Web.Application.Dtos.InvoiceDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Invoice;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Invoices;
using MercerStore.Web.Application.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Infrastructure.Data.Enum.Invoice;
using Moq;

namespace MercerStore.Tests.Services
{
    public class InvoiceServiceTests
    {
        private readonly InvoiceService _invoiceService;
        private readonly Mock<IInvoiceRepository> _invoiceRepostitoryMock;
        private readonly Mock<IRedisCacheService> _redisCacheServiceMock;
        private readonly Mock<IUserIdentifierService> _userIdentifierServiceMock;
        private readonly Mock<ILogService> _logServiceMock;
        private readonly Mock<IRequestContextService> _requestContextServiceMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;

        public InvoiceServiceTests()
        {

            _invoiceRepostitoryMock = new Mock<IInvoiceRepository>();
            _redisCacheServiceMock = new Mock<IRedisCacheService>();
            _userIdentifierServiceMock = new Mock<IUserIdentifierService>();
            _logServiceMock = new Mock<ILogService>();
            _requestContextServiceMock = new Mock<IRequestContextService>();
            _productRepositoryMock = new Mock<IProductRepository>();

            _invoiceService = new InvoiceService(
                _invoiceRepostitoryMock.Object,
                _redisCacheServiceMock.Object,
                _userIdentifierServiceMock.Object,
                _logServiceMock.Object,
                _requestContextServiceMock.Object,
                _productRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetFilteredInvoices_ShouldReturnPaginatedResult()
        {
            // Arrange
            var request = new InvoiceFilterRequest(1, 30);
            var invoiceDtos = new List<InvoiceDto> { new InvoiceDto { Id = 1, Status = "Ожидается" } };
            var paginatedResult = new PaginatedResultDto<InvoiceDto>(invoiceDtos, 1, 30);

            _redisCacheServiceMock
                .Setup(r => r.TryGetOrSetCacheAsync(It.IsAny<string>(), It.IsAny<Func<Task<PaginatedResultDto<InvoiceDto>>>>(), It.IsAny<bool>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _invoiceService.GetFilteredInvoices(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Items.Count());
        }

        [Fact]
        public async Task AddInvoice_ShouldReturnCreateInvoiceViewModel()
        {
            // Arrange
            var supplierId = 1;
            var managerId = "test-manager-123";
            _userIdentifierServiceMock.Setup(u => u.GetCurrentIdentifier()).Returns(managerId);

            var mockInvoice = new Invoice { ManagerId = managerId, SupplierId = supplierId, Status = InvoiceStatus.activ };
            _invoiceRepostitoryMock.Setup(r => r.GetInvoiceByManagerId(managerId)).ReturnsAsync(mockInvoice);
            _productRepositoryMock.Setup(p => p.GetIsUnassignedProducts()).ReturnsAsync(new List<Product> { new Product { Id = 1, Name = "Product 1" } });

            // Act
            var result = await _invoiceService.AddInvoice(supplierId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockInvoice.Id, result.InvoiceId);
            Assert.Equal(1, result.AvailableProducts.Count);
        }

        [Fact]
        public async Task AddInvoiceItem_ShouldAddItemAndReturnSuccess()
        {
            // Arrange
            var createInvoiceViewModel = new CreateInvoiceViewModel
            {
                InvoiceId = 1,
                ProductId = 1,
                Quantity = 2,
                ProductPrice = 100
            };

            var mockInvoice = new Invoice { Id = 1, Status = InvoiceStatus.activ };
            var mockProduct = new Product { Id = 1, Name = "Product 1" };

            _invoiceRepostitoryMock.Setup(r => r.GetInvoiceById(createInvoiceViewModel.InvoiceId)).ReturnsAsync(mockInvoice);
            _productRepositoryMock.Setup(p => p.GetProductByIdAsync(createInvoiceViewModel.ProductId)).ReturnsAsync(mockProduct);
            _invoiceRepostitoryMock.Setup(r => r.AddInvoiceItem(It.IsAny<InvoiceItem>())).Returns(Task.CompletedTask);

            // Act
            var result = await _invoiceService.AddInvoiceItem(createInvoiceViewModel);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(createInvoiceViewModel.ProductId, result.Data.ProductId);
        }

        [Fact]
        public async Task CloseInvoice_ShouldCloseInvoiceAndReturnSuccess()
        {
            // Arrange
            var invoiceId = 1;
            var notes = "Invoice closed.";
            var mockInvoice = new Invoice
            {
                Id = invoiceId,
                Status = InvoiceStatus.activ,
                InvoiceItems = new List<InvoiceItem>
                {
                    new InvoiceItem
                    { 
                        ProductId = 1,
                        Quantity = 2,
                        PurchasePrice = 50,
                        Product = new Product { Id = 1, SKU = "P001" }
                    }
                },

            };

            _invoiceRepostitoryMock.Setup(r => r.GetInvoiceById(invoiceId)).ReturnsAsync(mockInvoice);
            _productRepositoryMock.Setup(p => p.DecreaseProductStock(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(Task.CompletedTask);
            _invoiceRepostitoryMock.Setup(r => r.UpdateInvoice(It.IsAny<Invoice>())).Returns(Task.CompletedTask);

            // Act
            var result = await _invoiceService.CloseInvoice(invoiceId, notes);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateInvoice_ShouldUpdateInvoiceAndReturnInvoiceId()
        {
            // Arrange
            var updateInvoiceViewModel = new UpdateInvoiceViewModel
            {
                Id = 1,
                Status = InvoiceStatus.Closed,
                Notes = "Updated notes",
                InvoiceItems = new List<InvoiceItemViewModel>
                {
                    new InvoiceItemViewModel { Id = 1, Quantity = 2, PurchasePrice = 100 }
                }
            };

            var mockInvoice = new Invoice { Id = 1, Status = InvoiceStatus.activ };
            _invoiceRepostitoryMock.Setup(r => r.GetInvoiceById(updateInvoiceViewModel.Id)).ReturnsAsync(mockInvoice);
            _invoiceRepostitoryMock.Setup(r => r.UpdateInvoice(It.IsAny<Invoice>())).Returns(Task.CompletedTask);
            _invoiceRepostitoryMock.Setup(r => r.UpdateInvoiceItems(It.IsAny<List<InvoiceItemViewModel>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _invoiceService.UpdateInvoice(updateInvoiceViewModel);

            // Assert
            Assert.Equal(mockInvoice.Id, result);
        }
    }
}