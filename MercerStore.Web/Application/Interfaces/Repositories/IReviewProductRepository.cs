using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Dtos.MetricDto;
using MercerStore.Web.Application.Requests.Reviews;

namespace MercerStore.Web.Application.Interfaces.Repositories
{
    public interface IReviewProductRepository
    {
        Task<IEnumerable<Review>> GetAllProductReviews(int productId);
        Task<IEnumerable<Review>> GetAllReviewByUser(string userId);
        Task<int?> GetReviewId(string userId, int productId);
        Task<Review> GetReviewById(int? reviewId);
        Task<double> GetAvgRateProduct(int productId);
        Task<int> GetCountProductReviews(int productId);
        Task<Review> AddReview(Review review);
        Task UpdateReview(Review review);
        Task DeleteReview(int? reviewId);
        Task<(IEnumerable<Review>, int totalItems)> GetFilteredReviews(ReviewFilterRequest request);
        Task<ReviewMetricDto> GetReviewMetric();
    }
}
