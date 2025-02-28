using MercerStore.Data.Enum.User;
using MercerStore.Data.Enum;
using MercerStore.Dtos.ReviewDto;
using MercerStore.DTOs;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models.Products;
using MercerStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using MercerStore.Data.Enum.Review;
using Elasticsearch.Net;

namespace MercerStore.Controllers.Api
{
    [Authorize]
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewProductRepository _reviewProductRepository;
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly IRequestContextService _requestContextService;
        public ReviewsController(IReviewProductRepository reviewProductRepository,
            IUserIdentifierService userIdentifierService,
            IRequestContextService requestContextService)
        {
            _reviewProductRepository = reviewProductRepository;
            _userIdentifierService = userIdentifierService;
            _requestContextService = requestContextService;
        }

        [HttpGet("reviews/{productId}")]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            var reviews = await _reviewProductRepository.GetAllProductReviews(productId);
            if (reviews == null)
            {
                return BadRequest();
            }

            var reviewDto = reviews.Select(r => new ReviewDto
            {
                ProductId = r.ProductId,
                UserId = r.UserId,
                UserName = r.User?.UserName ?? "Гость",
                Value = r.Value,
                ReviewText = r.ReviewText,
                Date = r.Date
            }).ToList();

            return Ok(reviewDto);
        }

        [HttpGet("avg-rate/{productId}")]
        public async Task<IActionResult> GetAvgRateProduct(int productId)
        {
            var result = await _reviewProductRepository.GetAvgRateProduct(productId);
            return Ok(result);
        }

        [HttpGet("count-reviews/{productId}")]
        public async Task<IActionResult> GetCountProductReviews(int productId)
        {
            var result = await _reviewProductRepository.GetCountProductReviews(productId);
            return Ok(result);
        }

        [HttpGet("user-review/{productId}")]
        public async Task<IActionResult> GetReview(int productId)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var reviewId = await _reviewProductRepository.GetReviewId(userId, productId);
            var review = await _reviewProductRepository.GetReviewById(reviewId);
            return Ok(review);
        }

        [HttpPost("review")]
        [LogUserAction("User left a review", "review")]
        public async Task<IActionResult> AddReview(CreateReviewDto reviewDto)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var existingReview = await _reviewProductRepository.GetReviewId(userId, reviewDto.ProductId);

            if (existingReview != null)
            {
                return BadRequest("Пользователь уже оставил отзыв для этого товара.");
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

            var result = _reviewProductRepository.AddReview(review);
            var logDetails = new
            {
                review.ProductId,
                review.Value,
                review.ReviewText
            };

            _requestContextService.SetLogDetails(logDetails);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(review.Id);
        }

        [HttpDelete("review/{productId}")]
        [LogUserAction("User remove review", "review")]
        public async Task<IActionResult> RemoveReview(int productId)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var reviewId = await _reviewProductRepository.GetReviewId(userId, productId);
            await _reviewProductRepository.DeleteReview(reviewId);

            var logDetails = new
            {
                productId
            };

            _requestContextService.SetLogDetails(logDetails);

            return Ok(reviewId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("admin/review/{reviewId}")]
        [LogUserAction("User remove review", "review")]
        public async Task<IActionResult> RemoveReviewById(int reviewId)
        {
            await _reviewProductRepository.DeleteReview(reviewId);
            var managerId = _userIdentifierService.GetCurrentIdentifier();
            var logDetails = new
            {
                reviewId,
                managerId
            };
            _requestContextService.SetLogDetails(logDetails);

            return Ok(reviewId);
        }

        [HttpPut("review")]
        [LogUserAction("User update review", "review")]
        public async Task<IActionResult> UpdateReview([FromBody] CreateReviewDto reviewDto)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var reviewId = await _reviewProductRepository.GetReviewId(userId, reviewDto.ProductId);
            var review = await _reviewProductRepository.GetReviewById(reviewId);

            review.Value = reviewDto.Value;
            review.ReviewText = reviewDto.ReviewText;
            review.EditDateTime = DateTime.UtcNow;
            review.Edited = true;

            await _reviewProductRepository.UpdateReview(review);

            var logDetails = new
            {
                reviewDto.ProductId,
                review.Value,
                review.ReviewText
            };
            _requestContextService.SetLogDetails(logDetails);

            return Ok(reviewId);
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("reviews/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetFilteredReviews(int pageNumber, int pageSize, ReviewSortOrder? sortOrder, TimePeriod? period, ReviewFilter? filter, string? query)
        {

            var (reviewDtos, totalReviews) = await _reviewProductRepository.GetFilteredReviews(pageNumber, pageSize, sortOrder, period, filter, query);

            var result = new
            {
                Reviews = reviewDtos,
                TotalItems = totalReviews,
                TotalPages = (int)Math.Ceiling((double)totalReviews / pageSize)
            };

            return Ok(result);
        }
    }
}
