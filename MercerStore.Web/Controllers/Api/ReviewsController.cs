using MediatR;
using MercerStore.Web.Application.Dtos.Review;
using MercerStore.Web.Application.Handlers.Reviews.Commands;
using MercerStore.Web.Application.Handlers.Reviews.Queries;
using MercerStore.Web.Application.Requests.Reviews;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api;

[Authorize]
[Route("api/reviews")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("reviews/{productId}")]
    public async Task<IActionResult> GetProductReviews(int productId, CancellationToken ct)
    {
        var reviewDtos = await _mediator.Send(new GetProductReviewsQuery(productId), ct);
        return Ok(reviewDtos);
    }

    [HttpGet("avg-rate/{productId}")]
    public async Task<IActionResult> GetAvgRateProduct(int productId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAvgRateProductQuery(productId), ct);
        return Ok(result);
    }

    [HttpGet("count-reviews/{productId}")]
    public async Task<IActionResult> GetCountProductReviews(int productId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCountProductReviewsQuery(productId), ct);
        return Ok(result);
    }

    [HttpGet("user-review/{productId}")]
    public async Task<IActionResult> GetReview(int productId, CancellationToken ct)
    {
        var review = await _mediator.Send(new GetReviewQuery(productId), ct);
        return Ok(review);
    }

    [HttpPost("review")]
    public async Task<IActionResult> AddReview(CreateReviewDto reviewDto, CancellationToken ct)
    {
        var result = await _mediator.Send(new AddReviewCommand(reviewDto), ct);

        if (!result.IsSuccess) return BadRequest("Пользователь уже оставил отзыв для этого товара.");

        return Ok();
    }

    [HttpDelete("review/{productId}")]
    public async Task<IActionResult> RemoveReview(int productId, CancellationToken ct)
    {
        await _mediator.Send(new RemoveReviewCommand(productId), ct);
        return Ok();
    }

    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
    [HttpDelete("admin/review/{reviewId}")]
    public async Task<IActionResult> RemoveReviewById(int reviewId, CancellationToken ct)
    {
        await _mediator.Send(new RemoveReviewByIdCommand(reviewId), ct);
        return Ok();
    }

    [HttpPut("review")]
    public async Task<IActionResult> UpdateReview([FromBody] CreateReviewDto reviewDto, CancellationToken ct)
    {
        await _mediator.Send(new UpdateReviewCommand(reviewDto), ct);
        return Ok();
    }

    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
    [HttpGet("reviews")]
    public async Task<IActionResult> GetFilteredReviews([FromQuery] ReviewFilterRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetFilteredReviewsQuery(request), ct);

        return Ok(result);
    }
}