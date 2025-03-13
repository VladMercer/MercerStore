using CloudinaryDotNet.Actions;
using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Products;
using MercerStore.Web.Application.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Products;
using Moq;
namespace MercerStore.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ISKUService> _skuServiceMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IPhotoService> _photoServiceMock;
        private readonly Mock<IRedisCacheService> _redisCacheServiceMock;
        private readonly Mock<IElasticSearchService> _elasticsearchServiceMock;
        private readonly Mock<IRequestContextService> _requestContextServiceMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _skuServiceMock = new Mock<ISKUService>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _photoServiceMock = new Mock<IPhotoService>();
            _redisCacheServiceMock = new Mock<IRedisCacheService>();
            _elasticsearchServiceMock = new Mock<IElasticSearchService>();
            _requestContextServiceMock = new Mock<IRequestContextService>();

            _productService = new ProductService(
                _productRepositoryMock.Object,
                _skuServiceMock.Object,
                _redisCacheServiceMock.Object,
                Mock.Of<ICategoryService>(),
                Mock.Of<ISKUUpdater>(),
                _categoryRepositoryMock.Object,
                _photoServiceMock.Object,
                _elasticsearchServiceMock.Object,
                Mock.Of<IUserRepository>(),
                _requestContextServiceMock.Object

            );
        }
        [Fact]
        public async Task GetProduct_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new Product { Id = productId, Name = "Test Product" };

            _productRepositoryMock
                .Setup(repo => repo.GetProductByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _productService.GetProduct(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Test Product", result.Name);
        }
        [Fact]
        public async Task GetProductSku_ShouldReturnSku_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product" };
            var expectedSku = "SKU123";

            _productRepositoryMock
                .Setup(repo => repo.GetProductByIdAsync(productId))
                .ReturnsAsync(product);

            _skuServiceMock
                .Setup(skuService => skuService.GenerateSKU(product))
                .Returns(expectedSku);

            // Act
            var result = await _productService.GetProductSku(productId);

            // Assert
            Assert.Equal(expectedSku, result);
        }
        [Fact]
        public async Task GetAdminFilteredProducts_ShouldUseCache_WhenQueryIsDefault()
        {
            // Arrange
            var request = new ProductFilterRequest(null, 1, 30, null, null);
            var expectedProducts = new PaginatedResultDto<AdminProductDto>(
                new List<AdminProductDto> { new AdminProductDto { Id = 1, Name = "Product 1" } }, 1, 30
            );

            _redisCacheServiceMock
                .Setup(cache => cache.TryGetOrSetCacheAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<PaginatedResultDto<AdminProductDto>>>>(),
                    true,
                    It.IsAny<TimeSpan>()
                ))
                .ReturnsAsync(expectedProducts)
                .Verifiable();

            // Act
            var result = await _productService.GetAdminFilteredProducts(request);

            // Assert
            _redisCacheServiceMock.Verify();
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal("Product 1", result.Items.First().Name);
        }
        [Fact]
        public async Task GetAdminFilteredProducts_ShouldNotUseCache_WhenQueryIsNotDefault()
        {
            // Arrange
            var request = new ProductFilterRequest(2, 1, 30, null, null);
            var expectedProducts = new PaginatedResultDto<AdminProductDto>(
                new List<AdminProductDto> { new AdminProductDto { Id = 2, Name = "Product 2" } }, 1, 30
            );

            _redisCacheServiceMock
                .Setup(cache => cache.TryGetOrSetCacheAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<PaginatedResultDto<AdminProductDto>>>>(),
                    false,
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetAdminFilteredProducts(request);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal("Product 2", result.Items.First().Name);
        }

        [Fact]
        public async Task GetAdminFilteredProducts_ShouldCallRepository_WhenCacheIsEmpty()
        {
            // Arrange
            var request = new ProductFilterRequest(null, 1, 30, null, null);
            var expectedProducts = new PaginatedResultDto<AdminProductDto>(
                new List<AdminProductDto> { new AdminProductDto { Id = 3, Name = "Product 3" } }, 1, 30
            );

            _redisCacheServiceMock
                .Setup(cache => cache.TryGetOrSetCacheAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<PaginatedResultDto<AdminProductDto>>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<TimeSpan>()))
                .ReturnsAsync(expectedProducts);

            _productRepositoryMock
                .Setup(repo => repo.GetProductsAsync(It.IsAny<ProductFilterRequest>()))
                .ReturnsAsync((new List<Product> { new Product { Id = 3, Name = "Product 3" } }, 1));

            // Act
            var result = await _productService.GetAdminFilteredProducts(request);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal("Product 3", result.Items.First().Name);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnProductId_WhenProductIsCreated()
        {
            // Arrange
            var createViewModel = new CreateProductViewModel
            {
                Name = "Test Product",
                Price = 100,
                Description = "Test Description"
            };

            var imageUploadResult = new ImageUploadResult
            {
                Url = new Uri("http://image.com/test.jpg")
            };

            var product = new Product
            {
                Id = 10,
                Name = "Test Product",
                ProductPricing = new ProductPricing { OriginalPrice = 100 },
                ProductDescription = new ProductDescription { DescriptionText = "Test Description" },
                ProductStatus = new ProductStatus { Status = ProductStatuses.Available }
            };

            _productRepositoryMock
                .Setup(repo => repo.AddProduct(It.IsAny<Product>()))
                .Callback<Product>(p => p.Id = 10)
                .ReturnsAsync((Product p) => p);

            // Act
            var result = await _productService.CreateProduct(createViewModel, 1);

            // Assert
            Assert.Equal(10, result);
        }
    }
}
