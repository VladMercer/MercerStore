using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Reviews;

namespace MercerStore.Web.Application.Interfaces.Repositories;

public interface IReviewProductRepository
{
    Task<IEnumerable<Review>> GetAllProductReviews(int productId, CancellationToken ct);
    Task<IEnumerable<Review>> GetAllReviewByUser(string userId, CancellationToken ct);
    Task<int?> GetReviewId(string userId, int productId, CancellationToken ct);
    Task<Review> GetReviewById(int? reviewId, CancellationToken ct);
    Task<double> GetAvgRateProduct(int productId, CancellationToken ct);
    Task<int> GetCountProductReviews(int productId, CancellationToken ct);
    Task<Review> AddReview(Review review, CancellationToken ct);
    Task UpdateReview(Review review, CancellationToken ct);
    Task DeleteReview(int? reviewId, CancellationToken ct);

    Task<(IEnumerable<Review>, int totalItems)> GetFilteredReviews(ReviewFilterRequest request,
        CancellationToken ct);

    Task<ReviewMetricDto> GetReviewMetric(CancellationToken ct);
}
