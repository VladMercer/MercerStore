using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.ReviewDto;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Reviews;
using Microsoft.CodeAnalysis;

namespace MercerStore.Web.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewProductRepository _reviewProductRepository;

        public ReviewService(IReviewProductRepository reviewProductRepository)
        {
            _reviewProductRepository = reviewProductRepository;
        }

        public async Task UpdateReview(Review review)
        {
            await _reviewProductRepository.UpdateReview(review);
        }
        public async Task RemoveReviewById(int reviewId)
        {
            await _reviewProductRepository.DeleteReview(reviewId);
        }
        public async Task RemoveReview(int reviewId)
        {
            await _reviewProductRepository.DeleteReview(reviewId);
        }
        public async Task AddReview(Review review)
        {
            await _reviewProductRepository.AddReview(review);
        }
        public async Task<Review> GetReview(int productId, string userId)
        {
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
        public async Task<PaginatedResultDto<AdminReviewDto>> GetFilteredReviewsWithoutCache(ReviewFilterRequest request)
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

            return new PaginatedResultDto<AdminReviewDto>(reviewDtos, totalItems, request.PageSize);
        }
    }
}
