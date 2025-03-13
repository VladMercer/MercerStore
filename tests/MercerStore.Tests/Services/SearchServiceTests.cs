using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Search;
using MercerStore.Web.Application.Services;
using Moq;

namespace MercerStore.Tests.Services
{
    public class SearchServiceTests
    {
        private readonly Mock<IElasticSearchService> _elasticSearchServiceMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly SearchService _searchService;

        public SearchServiceTests()
        {
            _elasticSearchServiceMock = new Mock<IElasticSearchService>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _searchService = new SearchService(_elasticSearchServiceMock.Object, _productRepositoryMock.Object);
        }

        [Fact]
        public async Task SearchProduct_ShouldReturnCorrectlyMappedProducts()
        {
            // Arrange
            var request = new SearchFilterRequest("Test Product", null, 1, 9);

            var elasticSearchResults = new List<ProductIndexDto>
            {
                new() { Id = 1, Name = "Product A" },
                new() { Id = 2, Name = "Product B" }
            };

            var productEntities = new List<Product>
            {
                new()
                {
                    Id = 1,
                    Name = "Product A",
                    ProductPricing = new ProductPricing { OriginalPrice = 100, FixedDiscountPrice = 90 },
                    ProductDescription = new ProductDescription { DescriptionText = "Desc A" },
                    ProductStatus = new ProductStatus { InStock = 3, Status = ProductStatuses.Available },
                    MainImageUrl = "image1.jpg",
                    CategoryId = 10
                },
                new()
                {
                    Id = 2,
                    Name = "Product B",
                    ProductPricing = new ProductPricing { OriginalPrice = 200, FixedDiscountPrice = 180 },
                    ProductDescription = new ProductDescription { DescriptionText = "Desc B" },
                    ProductStatus = new ProductStatus { InStock = 2, Status = ProductStatuses.OutOfStock },
                    MainImageUrl = "image2.jpg",
                    CategoryId = 11
                }
            };

            _elasticSearchServiceMock
                .Setup(s => s.SearchProductsAsync("Test Product"))
                .ReturnsAsync(elasticSearchResults);

            _productRepositoryMock
                .Setup(r => r.GetProductsByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(productEntities);

            // Act
            var result = await _searchService.SearchProduct(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Products.Count);

            var product1 = result.Products[0];
            Assert.Equal(1, product1.Id);
            Assert.Equal(100, product1.Price);
            Assert.Equal(90, product1.DiscountedPrice);
            Assert.Equal("В наличии", product1.Status);
            Assert.Equal("image1.jpg", product1.MainImageUrl);

            var product2 = result.Products[1];
            Assert.Equal(2, product2.Id);
            Assert.Equal(200, product2.Price);
            Assert.Equal("Нет в наличии", product2.Status);
        }
    }
}