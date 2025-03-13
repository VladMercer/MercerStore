using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.SupplierDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Invoice;
using MercerStore.Web.Application.Requests.Suppliers;
using MercerStore.Web.Application.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Suppliers;
using Moq;

namespace MercerStore.Tests.Services
{
    public class SupplierServiceTests
    {
        private readonly Mock<IRequestContextService> _requestContextServiceMock;
        private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
        private readonly Mock<IRedisCacheService> _redisCacheServiceMock;
        private readonly Mock<ILogService> _logServiceMock;
        private readonly ISupplierService _supplierService;

        public SupplierServiceTests()
        {
            _requestContextServiceMock = new Mock<IRequestContextService>();
            _supplierRepositoryMock = new Mock<ISupplierRepository>();
            _redisCacheServiceMock = new Mock<IRedisCacheService>();
            _logServiceMock = new Mock<ILogService>();

            _supplierService = new SupplierService(
                _requestContextServiceMock.Object,
                _supplierRepositoryMock.Object,
                _redisCacheServiceMock.Object,
                _logServiceMock.Object
            );
        }

        [Fact]
        public async Task GetFilteredSuppliers_ShouldReturnCachedData()
        {
            // Arrange
            var request = new SupplierFilterRequest(1, 30, "");
            var cachedResult = new PaginatedResultDto<AdminSupplierDto>(
                new List<AdminSupplierDto> { new() { Id = 1, Name = "Supplier A" } },
                totalItems: 1,
                pageSize: 30
            );

            _redisCacheServiceMock
                .Setup(cache => cache.TryGetOrSetCacheAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<PaginatedResultDto<AdminSupplierDto>>>>(),
                    true,
                    TimeSpan.FromMinutes(10)
                ))
                .ReturnsAsync(cachedResult);

            // Act
            var result = await _supplierService.GetFilteredSuppliers(request);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(1, result.Items.ToList()[0].Id);

            _redisCacheServiceMock.Verify(s => s.TryGetOrSetCacheAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<PaginatedResultDto<AdminSupplierDto>>>>(),
                true,
                TimeSpan.FromMinutes(10)), Times.Once);
        }

        [Fact]
        public async Task RemoveSupplier_ShouldCallRepository()
        {
            // Arrange
            int supplierId = 1;

            _supplierRepositoryMock
                .Setup(r => r.RemoveSupplier(supplierId))
                .Returns(Task.CompletedTask);

            // Act
            await _supplierService.RemoveSupplier(supplierId);

            // Assert
            _supplierRepositoryMock.Verify(r => r.RemoveSupplier(supplierId), Times.Once);
        }

        [Fact]
        public async Task CreateSupplier_ShouldReturnNewSupplierId()
        {
            // Arrange
            var viewModel = new CreateSupplierViewModel
            {
                Name = "New Supplier",
                Address = "123 Street",
                Phone = "123456789",
                ContactPerson = "John Doe",
                Email = "supplier@example.com",
                IsCompany = true,
                TaxId = "TAX123"
            };

            var newSupplier = new Supplier { Id = 10, Name = viewModel.Name };

            _supplierRepositoryMock
                .Setup(r => r.AddSupplier(It.IsAny<Supplier>()))
                .ReturnsAsync(newSupplier);

            // Act
            var result = await _supplierService.CreateSupplier(viewModel);

            // Assert
            Assert.Equal(10, result);
            _supplierRepositoryMock.Verify(r => r.AddSupplier(It.IsAny<Supplier>()), Times.Once);
        }

        [Fact]
        public async Task GetUpdateSupplierViewModel_ShouldReturnSupplier()
        {
            // Arrange
            int supplierId = 5;
            var supplier = new Supplier
            {
                Id = supplierId,
                Name = "Test Supplier",
                Address = "Test Address",
                Phone = "123456",
                ContactPerson = "Test Contact",
                Email = "test@example.com",
                IsCompany = true,
                TaxId = "TAX321"
            };

            _supplierRepositoryMock
                .Setup(r => r.GetSupplierById(supplierId))
                .ReturnsAsync(supplier);

            // Act
            var result = await _supplierService.GetUpdateSupplierViewModel(supplierId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(supplierId, result.Id);
            Assert.Equal("Test Supplier", result.Name);
            Assert.Equal("Test Address", result.Address);
            Assert.Equal("123456", result.Phone);
        }

        [Fact]
        public async Task UpdateSupplier_ShouldReturnSuccess_WhenSupplierExists()
        {
            // Arrange
            var updateViewModel = new UpdateSupplierViewModel
            {
                Id = 3,
                Name = "Updated Supplier",
                Address = "New Address",
                Phone = "987654321",
                ContactPerson = "Jane Doe",
                Email = "updated@example.com",
                IsCompany = false,
                TaxId = "TAX999"
            };

            var existingSupplier = new Supplier
            {
                Id = 3,
                Name = "Old Supplier",
                Address = "Old Address",
                Phone = "000000",
                ContactPerson = "Old Contact",
                Email = "old@example.com",
                IsCompany = true,
                TaxId = "TAX000"
            };

            _supplierRepositoryMock
                .Setup(r => r.GetSupplierById(updateViewModel.Id))
                .ReturnsAsync(existingSupplier);

            _supplierRepositoryMock
                .Setup(r => r.UpdateSupplier(existingSupplier))
                .ReturnsAsync(existingSupplier.Id);

            // Act
            var result = await _supplierService.UpdateSupplier(updateViewModel);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data);
            _supplierRepositoryMock.Verify(r => r.UpdateSupplier(existingSupplier), Times.Once);
        }

        [Fact]
        public async Task UpdateSupplier_ShouldReturnFailure_WhenSupplierNotFound()
        {
            // Arrange
            var updateViewModel = new UpdateSupplierViewModel { Id = 99 };

            _supplierRepositoryMock
                .Setup(r => r.GetSupplierById(updateViewModel.Id))
                .ReturnsAsync((Supplier)null);

            // Act
            var result = await _supplierService.UpdateSupplier(updateViewModel);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Поставщик не найден", result.ErrorMessage);
            _supplierRepositoryMock.Verify(r => r.UpdateSupplier(It.IsAny<Supplier>()), Times.Never);
        }
    }
}