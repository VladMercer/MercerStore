using Moq;
using MercerStore.Web.Application.Services;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Tests.Services
{
    public class HomeServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly HomeService _homeService;

        public HomeServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _homeService = new HomeService(_mockProductRepository.Object);
        }

        [Fact]
        public async Task GetHomePageProduct_ShouldReturnCorrectViewModel_WhenValidData()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", MainImageUrl = "url1", ProductDescription = new ProductDescription { DescriptionText = "Description 1" }, ProductPricing = new ProductPricing { OriginalPrice = 100, FixedDiscountPrice = 90 } },
                new Product { Id = 2, Name = "Product 2", MainImageUrl = "url2", ProductDescription = new ProductDescription { DescriptionText = "Description 2" }, ProductPricing = new ProductPricing { OriginalPrice = 200, FixedDiscountPrice = 180 } }
            };

            var randomProducts = new List<Product>
            {
                new Product { Id = 3, Name = "Random Product 1", MainImageUrl = "url3", ProductDescription = new ProductDescription { DescriptionText = "Random Description 1" }, ProductPricing = new ProductPricing { OriginalPrice = 150, FixedDiscountPrice = 140 } },
                new Product { Id = 4, Name = "Random Product 2", MainImageUrl = "url4", ProductDescription = new ProductDescription { DescriptionText = "Random Description 2" }, ProductPricing = new ProductPricing { OriginalPrice = 250, FixedDiscountPrice = 230 } }
            };

            _mockProductRepository.Setup(repo => repo.GetLastProductsAsync(9)).ReturnsAsync(products);
            _mockProductRepository.Setup(repo => repo.GetRandomProductsAsync(9)).ReturnsAsync(randomProducts);

            // Act
            var result = await _homeService.GetHomePageProduct();

            // Assert
            Assert.Equal(2, result.Products.Count());
            Assert.Equal("Product 1", result.Products.First().Name);
            Assert.Equal(100, result.Products.First().Price);
            Assert.Equal("Description 1", result.Products.First().Description);

            Assert.Equal(2, result.RandomProducts.Count());
            Assert.Equal("Random Product 1", result.RandomProducts.First().Name);
            Assert.Equal(150, result.RandomProducts.First().Price);
            Assert.Equal("Random Description 1", result.RandomProducts.First().Description);
        }

        [Fact]
        public async Task GetHomePageProduct_ShouldReturnEmptyLists_WhenNoProducts()
        {
            // Arrange
            var emptyProducts = new List<Product>();
            _mockProductRepository.Setup(repo => repo.GetLastProductsAsync(9)).ReturnsAsync(emptyProducts);
            _mockProductRepository.Setup(repo => repo.GetRandomProductsAsync(9)).ReturnsAsync(emptyProducts);

            // Act
            var result = await _homeService.GetHomePageProduct();

            // Assert
            Assert.Empty(result.Products);
            Assert.Empty(result.RandomProducts);
        }
    }
}