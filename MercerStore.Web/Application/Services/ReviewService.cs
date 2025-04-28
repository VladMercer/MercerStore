using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Dtos.Review;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Reviews;

namespace MercerStore.Web.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewProductRepository _reviewProductRepository;

    public ReviewService(IReviewProductRepository reviewProductRepository)
    {
        _reviewProductRepository = reviewProductRepository;
    }

    public async Task UpdateReview(Review review, CancellationToken ct)
    {
        await _reviewProductRepository.UpdateReview(review, ct);
    }

    public async Task RemoveReviewById(int reviewId, CancellationToken ct)
    {
        await _reviewProductRepository.DeleteReview(reviewId, ct);
    }

    public async Task RemoveReview(int reviewId, CancellationToken ct)
    {
        await _reviewProductRepository.DeleteReview(reviewId, ct);
    }

    public async Task AddReview(Review review, CancellationToken ct)
    {
        await _reviewProductRepository.AddReview(review, ct);
    }

    public async Task<Review> GetReview(int productId, string userId, CancellationToken ct)
    {
        var reviewId = await _reviewProductRepository.GetReviewId(userId, productId, ct);
        var review = await _reviewProductRepository.GetReviewById(reviewId, ct);
        return review;
    }

    public async Task<int> GetCountProductReviews(int productId, CancellationToken ct)
    {
        return await _reviewProductRepository.GetCountProductReviews(productId, ct);
    }

    public async Task<double> GetAvgRateProduct(int productId, CancellationToken ct)
    {
        return await _reviewProductRepository.GetAvgRateProduct(productId, ct);
    }

    public async Task<IEnumerable<ReviewDto>> GetProductReviews(int productId, CancellationToken ct)
    {
        var reviews = await _reviewProductRepository.GetAllProductReviews(productId, ct);

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

    public async Task<PaginatedResultDto<AdminReviewDto>> GetFilteredReviewsWithoutCache(ReviewFilterRequest request,
        CancellationToken ct)
    {
        var (reviews, totalItems) = await _reviewProductRepository.GetFilteredReviews(request, ct);

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