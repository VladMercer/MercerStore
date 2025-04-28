using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Dtos.Review;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Reviews;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IReviewService
{
    Task<PaginatedResultDto<AdminReviewDto>> GetFilteredReviewsWithoutCache(ReviewFilterRequest request,
        CancellationToken ct);

    Task UpdateReview(Review review, CancellationToken ct);
    Task RemoveReviewById(int reviewId, CancellationToken ct);
    Task RemoveReview(int reviewId, CancellationToken ct);
    Task AddReview(Review review, CancellationToken ct);
    Task<Review> GetReview(int productId, string userId, CancellationToken ct);
    Task<int> GetCountProductReviews(int productId, CancellationToken ct);
    Task<double> GetAvgRateProduct(int productId, CancellationToken ct);
    Task<IEnumerable<ReviewDto>> GetProductReviews(int productId, CancellationToken ct);
}