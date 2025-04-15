using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.ReviewDto;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Reviews;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<PaginatedResultDto<AdminReviewDto>> GetFilteredReviewsWithoutCache(ReviewFilterRequest request);
        Task UpdateReview(Review review);
        Task RemoveReviewById(int reviewId);
        Task RemoveReview(int reviewId);
        Task AddReview(Review review);
        Task<Review> GetReview(int productId, string userId);
        Task<int> GetCountProductReviews(int productId);
        Task<double> GetAvgRateProduct(int productId);
        Task<IEnumerable<ReviewDto>> GetProductReviews(int productId);
    }
}
