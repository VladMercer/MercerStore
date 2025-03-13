using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.ReviewDto;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Reviews;
using MercerStore.Web.Application.Services;
using Moq;

namespace MercerStore.Tests.Services
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewProductRepository> _reviewRepositoryMock;
        private readonly Mock<IUserIdentifierService> _userIdentifierServiceMock;
        private readonly Mock<IRequestContextService> _requestContextServiceMock;
        private readonly Mock<IRedisCacheService> _redisCacheServiceMock;
        private readonly ReviewService _reviewService;

        public ReviewServiceTests()
        {
            _reviewRepositoryMock = new Mock<IReviewProductRepository>();
            _userIdentifierServiceMock = new Mock<IUserIdentifierService>();
            _requestContextServiceMock = new Mock<IRequestContextService>();
            _redisCacheServiceMock = new Mock<IRedisCacheService>();

            _reviewService = new ReviewService(
                _reviewRepositoryMock.Object,
                _userIdentifierServiceMock.Object,
                _requestContextServiceMock.Object,
                _redisCacheServiceMock.Object);
        }

        [Fact]
        public async Task GetFilteredReviews_ShouldReturnCachedReviews_WhenCacheExists()
        {
            // Arrange
            var request = new ReviewFilterRequest { PageNumber = 1, PageSize = 30, Query = "" };
            var cachedResult = new PaginatedResultDto<AdminReviewDto>(new List<AdminReviewDto>(), 0, 30);

            _redisCacheServiceMock
                .Setup(cache => cache.TryGetOrSetCacheAsync(It.IsAny<string>(), It.IsAny<Func<Task<PaginatedResultDto<AdminReviewDto>>>>(), true, TimeSpan.FromMinutes(10)))
                .ReturnsAsync(cachedResult);

            // Act
            var result = await _reviewService.GetFilteredReviews(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalItems);
        }

        [Fact]
        public async Task AddReview_ShouldReturnReviewId_WhenReviewIsAdded()
        {
            // Arrange
            var userId = "test-user-123";
            var productId = 1;
            var reviewDto = new CreateReviewDto { ProductId = productId, ReviewText = "Great!", Value = 5 };

            _userIdentifierServiceMock.Setup(service => service.GetCurrentIdentifier()).Returns(userId);
            _reviewRepositoryMock.Setup(repo => repo.GetReviewId(userId, productId)).ReturnsAsync((int?)null);

            var review = new Review
            {
                Id = 10,
                UserId = userId,
                ProductId = productId,
                ReviewText = reviewDto.ReviewText,
                Value = reviewDto.Value,
                Date = DateTime.UtcNow,
                EditDateTime = DateTime.UtcNow
            };

            _reviewRepositoryMock.Setup(repo => repo.AddReview(It.IsAny<Review>())).ReturnsAsync(review);

            // Act
            var result = await _reviewService.AddReview(reviewDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(10, result.Data);
        }

        [Fact]
        public async Task AddReview_ShouldReturnError_WhenReviewAlreadyExists()
        {
            // Arrange
            var userId = "test-user-123";
            var productId = 1;
            var reviewDto = new CreateReviewDto { ProductId = productId, ReviewText = "Nice!", Value = 4 };

            _userIdentifierServiceMock.Setup(service => service.GetCurrentIdentifier()).Returns(userId);
            _reviewRepositoryMock.Setup(repo => repo.GetReviewId(userId, productId)).ReturnsAsync(5); 

            // Act
            var result = await _reviewService.AddReview(reviewDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Отзыв уже существует",result.ErrorMessage);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateReview_ShouldUpdateReview_WhenReviewExists()
        {
            // Arrange
            var userId = "test-user-123";
            var productId = 1;
            var reviewId = 10;
            var dto = new CreateReviewDto { ProductId = productId, ReviewText = "Updated review", Value = 4 };

            _userIdentifierServiceMock.Setup(service => service.GetCurrentIdentifier()).Returns(userId);
            _reviewRepositoryMock.Setup(repo => repo.GetReviewId(userId, productId)).ReturnsAsync(reviewId);

            var review = new Review { Id = reviewId, ProductId = productId, UserId = userId, ReviewText = "Old review", Value = 3 };
            _reviewRepositoryMock.Setup(repo => repo.GetReviewById(reviewId)).ReturnsAsync(review);

            // Act
            var result = await _reviewService.UpdateReview(dto);

            // Assert
            Assert.Equal(reviewId, result);
            _reviewRepositoryMock.Verify(repo => repo.UpdateReview(It.IsAny<Review>()), Times.Once);
        }

        [Fact]
        public async Task RemoveReview_ShouldDeleteReview_WhenReviewExists()
        {
            // Arrange
            var userId = "test-user-123";
            var productId = 1;
            var reviewId = 10;

            _userIdentifierServiceMock.Setup(service => service.GetCurrentIdentifier()).Returns(userId);
            _reviewRepositoryMock.Setup(repo => repo.GetReviewId(userId, productId)).ReturnsAsync(reviewId);

            // Act
            var result = await _reviewService.RemoveReview(productId);

            // Assert
            Assert.Equal(reviewId, result);
            _reviewRepositoryMock.Verify(repo => repo.DeleteReview(reviewId), Times.Once);
        }

        [Fact]
        public async Task RemoveReviewById_ShouldDeleteReview()
        {
            // Arrange
            var reviewId = 10;

            // Act
            await _reviewService.RemoveReviewById(reviewId);

            // Assert
            _reviewRepositoryMock.Verify(repo => repo.DeleteReview(reviewId), Times.Once);
        }

        [Fact]
        public async Task GetReview_ShouldReturnReview_WhenExists()
        {
            // Arrange
            var userId = "test-user-123";
            var productId = 1;
            var reviewId = 10;

            var review = new Review { Id = reviewId, ProductId = productId, UserId = userId, ReviewText = "Sample review", Value = 5 };

            _userIdentifierServiceMock.Setup(service => service.GetCurrentIdentifier()).Returns(userId);
            _reviewRepositoryMock.Setup(repo => repo.GetReviewId(userId, productId)).ReturnsAsync(reviewId);
            _reviewRepositoryMock.Setup(repo => repo.GetReviewById(reviewId)).ReturnsAsync(review);

            // Act
            var result = await _reviewService.GetReview(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Sample review", result.ReviewText);
        }

        [Fact]
        public async Task GetCountProductReviews_ShouldReturnCorrectCount()
        {
            // Arrange
            var productId = 1;
            _reviewRepositoryMock.Setup(repo => repo.GetCountProductReviews(productId)).ReturnsAsync(5);

            // Act
            var result = await _reviewService.GetCountProductReviews(productId);

            // Assert
            Assert.Equal(5, result);
        }
    }
}
