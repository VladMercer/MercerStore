using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MercerStore.Web.Application.Dtos.ReviewDto;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Reviews;

namespace MercerStore.Web.Controllers.Api
{
    [Authorize]
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
       
        private readonly IReviewService _reviewService;
        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("reviews/{productId}")]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            var reviewDtos = await _reviewService.GetProductReviews(productId);
            return Ok(reviewDtos);
        }

        [HttpGet("avg-rate/{productId}")]
        public async Task<IActionResult> GetAvgRateProduct(int productId)
        {
            var result = await _reviewService.GetAvgRateProduct(productId);
            return Ok(result);
        }

        [HttpGet("count-reviews/{productId}")]
        public async Task<IActionResult> GetCountProductReviews(int productId)
        {
            var result = await _reviewService.GetCountProductReviews(productId);
            return Ok(result);
        }

        [HttpGet("user-review/{productId}")]
        public async Task<IActionResult> GetReview(int productId)
        {
            var review = await _reviewService.GetReview(productId);
            return Ok(review);
        }

        [HttpPost("review")]
        [LogUserAction("User left a review", "review")]
        public async Task<IActionResult> AddReview(CreateReviewDto reviewDto)
        {
            var result = await _reviewService.AddReview(reviewDto);

            if (!result.IsSuccess)
            {
                return BadRequest("Пользователь уже оставил отзыв для этого товара.");
            }

            return Ok(result.Data);
        }

        [HttpDelete("review/{productId}")]
        [LogUserAction("User remove review", "review")]
        public async Task<IActionResult> RemoveReview(int productId)
        {
            var reviewId = await _reviewService.RemoveReview(productId);
            return Ok(reviewId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("admin/review/{reviewId}")]
        [LogUserAction("User remove review", "review")]
        public async Task<IActionResult> RemoveReviewById(int reviewId)
        {
            await _reviewService.RemoveReviewById(reviewId);
            return Ok(reviewId);
        }

        [HttpPut("review")]
        [LogUserAction("User update review", "review")]
        public async Task<IActionResult> UpdateReview([FromBody] CreateReviewDto reviewDto)
        {
            var reviewId = await _reviewService.UpdateReview(reviewDto);
            return Ok(reviewId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("reviews")]
        public async Task<IActionResult> GetFilteredReviews([FromQuery] ReviewFilterRequest request)
        {
            var result = await _reviewService.GetFilteredReviews(request);
   
            return Ok(result);
        }
    }
}
