using MercerStore.DTOs;
using MercerStore.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewProductRepository _reviewProductRepository;

        public ReviewsController(IReviewProductRepository reviewProductRepository)
        {
            _reviewProductRepository = reviewProductRepository;
        }
        [HttpGet("reviews/{productId}")]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            var reviews = await _reviewProductRepository.GetAllProductReviews(productId);
            if (reviews == null)
            {
                return BadRequest();
            }

            var reviewDto = reviews.Select( r => new ReviewDto
            {
                ProductId = r.ProductId,
                UserId = r.UserId,
                UserName = r.User.UserName,
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

        [HttpGet("userId")]
        public IActionResult GetCurrentUserId()
        {
            var currentUser = HttpContext.User.GetUserId();
            if (currentUser == null)
            {
                return BadRequest();
            }
            return Ok(currentUser);
        }

        [HttpGet("user-review/{productId}")]
        public async Task<IActionResult> GetReview(int productId)
        {
            var userId = HttpContext.User.GetUserId();
            var reviewId = await _reviewProductRepository.GetReviewId(userId, productId);
            var review = await _reviewProductRepository.GetReviewById(reviewId);
            return Ok(review);
        }
        [HttpPost("review")]
        public async Task<IActionResult> AddReview(CreateReviewDto reviewDto)
        {
            var userId = HttpContext.User.GetUserId();
            var existingReview = await _reviewProductRepository.GetReviewId(userId, reviewDto.ProductId);
            if (existingReview != 0)
            {
                return BadRequest("Пользователь уже оставил отзыв для этого товара.");
            }
            var review = new Review
            {
                UserId = userId,
                ReviewText = reviewDto.ReviewText,
                ProductId = reviewDto.ProductId,
                Value = reviewDto.Value,
                Date = DateTime.UtcNow
            };
            var result = _reviewProductRepository.AddReview(review);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("review/{productId}")]
        public async Task<IActionResult> RemoveReview(int productId)
        {
            var userId = HttpContext.User.GetUserId();
            var reviewId = await _reviewProductRepository.GetReviewId(userId, productId);
            await _reviewProductRepository.DeleteReview(reviewId);
            return Ok();
        }

        [HttpPut("review")]
        public async Task<IActionResult> UpdateReview([FromBody]CreateReviewDto reviewDto)
        {
            var userId = HttpContext.User.GetUserId();
            var reviewId = await _reviewProductRepository.GetReviewId(userId, reviewDto.ProductId);
            var review = await _reviewProductRepository.GetReviewById(reviewId);

            review.Value = reviewDto.Value;
            review.ReviewText = reviewDto.ReviewText;

            await _reviewProductRepository.UpdateReview(review);
            return Ok();


        }
    }
}
