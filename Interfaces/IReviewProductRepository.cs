using MercerStore.Data.Enum.Review;
using MercerStore.Data.Enum;
using MercerStore.Dtos.ReviewDto;
using MercerStore.Models.Products;

namespace MercerStore.Interfaces
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
        Task<(IEnumerable<AdminReviewDto>, int totalItems)> GetFilteredReviews(
        int pageNumber,
        int pageSize,
        ReviewSortOrder? sortOrder,
        TimePeriod? timePeriod,
        ReviewFilter? filter,
        string? query);
        Task<object> GetReviewMetric();
    }
}
