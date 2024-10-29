using MercerStore.Models;

namespace MercerStore.Interfaces
{
    public interface IReviewProductRepository
    {
        Task<IEnumerable<Review>> GetAllProductReviews(int productId);
        Task<IEnumerable<Review>> GetAllReviewByUser(string userId);
        Task<int> GetReviewId(string userId, int productId);
        Task<Review> GetReviewById(int reviewId);
        Task<double> GetAvgRateProduct(int productId);
        Task<int> GetCountProductReviews(int productId);
        Task AddReview(Review review);
        Task UpdateReview(Review review);
        Task DeleteReview(int reviewId);
    }
}
