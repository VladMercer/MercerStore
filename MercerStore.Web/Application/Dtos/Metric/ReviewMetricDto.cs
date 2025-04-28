namespace MercerStore.Web.Application.Dtos.Metric;

public class ReviewMetricDto
{
    public int Total { get; set; }
    public double AverageRating { get; set; }
    public NewReviewsDto? NewReviews { get; set; }
    public IList<TopRatedProductDto>? TopRatedProducts { get; set; }
}