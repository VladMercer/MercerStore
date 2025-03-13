using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.ReviewDto;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Reviews;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<PaginatedResultDto<AdminReviewDto>> GetFilteredReviews(ReviewFilterRequest request);
        Task<int?> UpdateReview(CreateReviewDto dto);
        Task RemoveReviewById(int reviewId);
        Task<int?> RemoveReview(int productId);
        Task<Result<int>> AddReview(CreateReviewDto reviewDto);
        Task<Review> GetReview(int productId);
        Task<int> GetCountProductReviews(int productId);
        Task<double> GetAvgRateProduct(int productId);
        Task<IEnumerable<ReviewDto>> GetProductReviews(int productId);
    }
}
