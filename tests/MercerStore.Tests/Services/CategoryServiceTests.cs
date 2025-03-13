using Moq;
using MercerStore.Web.Application.Services;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Requests.Categories;
using MercerStore.Web.Application.ViewModels.Categories;
using MercerStore.Web.Application.Dtos.ResultDto;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;

namespace MercerStore.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IRedisCacheService> _mockRedisCacheService;
        private readonly Mock<IPhotoService> _mockPhotoService;
        private readonly Mock<IRequestContextService> _mockRequestContextService;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockRedisCacheService = new Mock<IRedisCacheService>();
            _mockPhotoService = new Mock<IPhotoService>();
            _mockRequestContextService = new Mock<IRequestContextService>();

            _categoryService = new CategoryService(
                _mockProductRepository.Object,
                _mockCategoryRepository.Object,
                _mockRedisCacheService.Object,
                _mockPhotoService.Object,
                _mockRequestContextService.Object
            );
        }

        [Fact]
        public async Task GetFilteredProducts_ShouldReturnPaginatedProducts_WhenValidRequest()
        {
            // Arrange
            var categoryId = 1;
            var request = new CateroryFilterRequest(categoryId, 1, 9, "price_asc", 10, 100);

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", CategoryId = categoryId, ProductPricing = new ProductPricing { OriginalPrice = 50 } },
                new Product { Id = 2, Name = "Product2", CategoryId = categoryId, ProductPricing = new ProductPricing { OriginalPrice = 30 } }
            };

            _mockCategoryRepository.Setup(repo => repo.GetProductsByCategoryId(categoryId)).ReturnsAsync(products);
            _mockRedisCacheService.Setup(redis => redis.TryGetOrSetCacheAsync(It.IsAny<string>(), It.IsAny<Func<Task<PaginatedResultDto<ProductDto>>>>(), It.IsAny<bool>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(new PaginatedResultDto<ProductDto>(products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.ProductPricing.OriginalPrice,
                    MainImageUrl = "url",
                    CategoryId = p.CategoryId
                }), 2, 9));

            // Act
            var result = await _categoryService.GetFilteredProducts(request);

            // Assert
            Assert.Equal(2, result.Items.Count());
            Assert.Equal("Product1", result.Items.First().Name);
            Assert.Equal(2, result.TotalItems);
        }

        [Fact]
        public async Task GetPriceRange_ShouldReturnValidPriceRange_WhenProductsExist()
        {
            // Arrange
            var categoryId = 1;
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", CategoryId = categoryId, ProductPricing = new ProductPricing { OriginalPrice = 50, FixedDiscountPrice = 40 } },
                new Product { Id = 2, Name = "Product2", CategoryId = categoryId, ProductPricing = new ProductPricing { OriginalPrice = 30, FixedDiscountPrice = 25 } }
            };

            _mockCategoryRepository.Setup(repo => repo.GetProductsByCategoryId(categoryId)).ReturnsAsync(products);

            // Act
            var result = await _categoryService.GetPriceRange(categoryId);

            // Assert
            Assert.Equal(25, result.MinPrice);
            Assert.Equal(40, result.MaxPrice);
        }

        [Fact]
        public async Task AddCategory_ShouldReturnCategoryId_WhenValidCategoryData()
        {
            // Arrange
            var createCategoryViewModel = new CreateCategoryViewModel
            {
                Name = "New Category",
                Description = "Category description",
                CategoryImage = null 
            };

            _mockPhotoService.Setup(photo => photo.AddPhotoAsync(It.IsAny<IFormFile>())).ReturnsAsync(new ImageUploadResult { Url = new Uri("https://example.com/photo.jpg") });
            _mockCategoryRepository.Setup(repo => repo.AddCategory(It.IsAny<Category>())).ReturnsAsync(1);

            // Act
            var result = await _categoryService.AddCategory(createCategoryViewModel);

            // Assert
            Assert.Equal(1, result);
            _mockCategoryRepository.Verify(repo => repo.AddCategory(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task GetCategoryPageViewModel_ShouldReturnCorrectViewModel()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Category1", Description = "Category Description" };
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetCategoryPageViewModel(categoryId);

            // Assert
            Assert.Equal(category, result.Category);
            Assert.Equal(categoryId, result.SelectedCategoryId);
        }
    }
}