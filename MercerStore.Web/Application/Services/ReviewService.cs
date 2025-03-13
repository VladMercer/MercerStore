using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.ReviewDto;
using MercerStore.Web.Application.Requests.Reviews;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using Microsoft.CodeAnalysis;
using MercerStore.Web.Infrastructure.Helpers;
using Nest;

namespace MercerStore.Web.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewProductRepository _reviewProductRepository;
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly IRequestContextService _requestContextService;
        private readonly IRedisCacheService _redisCacheService;

        public ReviewService(
            IReviewProductRepository reviewProductRepository,
            IUserIdentifierService userIdentifierService,
            IRequestContextService requestContextService,
            IRedisCacheService redisCacheService)
        {
            _reviewProductRepository = reviewProductRepository;
            _userIdentifierService = userIdentifierService;
            _requestContextService = requestContextService;
            _redisCacheService = redisCacheService;
        }
        public async Task<PaginatedResultDto<AdminReviewDto>> GetFilteredReviews(ReviewFilterRequest request)
        {

            bool isDefaultQuery =
            request.PageNumber == 1 &&
            request.PageSize == 30 &&
            !request.SortOrder.HasValue &&
            !request.Filter.HasValue &&
            request.Query == "";

            string cacheKey = $"reviews:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => FetchFilteredReviews(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
        public async Task<int?> UpdateReview(CreateReviewDto dto)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var reviewId = await _reviewProductRepository.GetReviewId(userId, dto.ProductId);
            var review = await _reviewProductRepository.GetReviewById(reviewId);

            review.Value = dto.Value;
            review.ReviewText = dto.ReviewText;
            review.EditDateTime = DateTime.UtcNow;
            review.Edited = true;

            await _reviewProductRepository.UpdateReview(review);

            var logDetails = new
            {
                dto.ProductId,
                review.Value,
                review.ReviewText
            };
            _requestContextService.SetLogDetails(logDetails);

            return reviewId;
        }
        public async Task RemoveReviewById(int reviewId)
        {
            await _reviewProductRepository.DeleteReview(reviewId);
            var managerId = _userIdentifierService.GetCurrentIdentifier();
            var logDetails = new
            {
                reviewId,
                managerId
            };
            _requestContextService.SetLogDetails(logDetails);
        }
        public async Task<int?> RemoveReview(int productId)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var reviewId = await _reviewProductRepository.GetReviewId(userId, productId);
            await _reviewProductRepository.DeleteReview(reviewId);

            var logDetails = new
            {
                productId
            };

            _requestContextService.SetLogDetails(logDetails);

            return reviewId;
        }
        public async Task<Result<int>> AddReview(CreateReviewDto reviewDto)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var existingReview = await _reviewProductRepository.GetReviewId(userId, reviewDto.ProductId);

            if (existingReview != null)
            {
                return Result<int>.Failure("Отзыв уже существует");
            }

            var review = new Review
            {
                UserId = userId,
                ReviewText = reviewDto.ReviewText,
                ProductId = reviewDto.ProductId,
                Value = reviewDto.Value,
                Date = DateTime.UtcNow,
                EditDateTime = DateTime.UtcNow
            };

            var result = await _reviewProductRepository.AddReview(review);
            var logDetails = new
            {
                review.ProductId,
                review.Value,
                review.ReviewText
            };

            _requestContextService.SetLogDetails(logDetails);

            if (result == null)
            {
                return Result<int>.Failure("Ошибка добавления отзыва");
            }

            return Result<int>.Success(result.Id);

        }
        public async Task<Review> GetReview(int productId)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var reviewId = await _reviewProductRepository.GetReviewId(userId, productId);
            var review = await _reviewProductRepository.GetReviewById(reviewId);

            return review;
        }
        public async Task<int> GetCountProductReviews(int productId)
        {
            return await _reviewProductRepository.GetCountProductReviews(productId);
        }
        public async Task<double> GetAvgRateProduct(int productId)
        {
            return await _reviewProductRepository.GetAvgRateProduct(productId);
        }
        public async Task<IEnumerable<ReviewDto>> GetProductReviews(int productId)
        {
            var reviews = await _reviewProductRepository.GetAllProductReviews(productId);
         
            var reviewDtos = reviews.Select(r => new ReviewDto
            {
                ProductId = r.ProductId,
                UserId = r.UserId,
                UserName = r.User?.UserName ?? "Гость",
                Value = r.Value,
                ReviewText = r.ReviewText,
                Date = r.Date
            }).ToList();

            return reviewDtos;

        }
        private async Task<PaginatedResultDto<AdminReviewDto>> FetchFilteredReviews(ReviewFilterRequest request)
        {
            var (reviews, totalItems) = await _reviewProductRepository.GetFilteredReviews(request);

            var reviewDtos = reviews.Select(u => new AdminReviewDto
            {
                Id = u.Id,
                ProductId = u.ProductId,
                UserId = u.UserId,
                Date = u.Date,
                EditDate = u.EditDateTime,
                productImgUrl = u.Product.MainImageUrl,
                ReviewText = u.ReviewText,
                UserName = u.User.UserName,
                Email = u.User.Email,
                Value = u.Value
            }).ToList();

            var result = new PaginatedResultDto<AdminReviewDto>(reviewDtos, totalItems, request.PageSize);

            return result;
        }
    }
}
