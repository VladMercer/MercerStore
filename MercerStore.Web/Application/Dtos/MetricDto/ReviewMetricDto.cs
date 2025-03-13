namespace MercerStore.Web.Application.Dtos.MetricDto
{
    public class ReviewMetricDto
    {
        public int Total { get; set; }
        public double AverageRating { get; set; }
        public NewReviewsDto NewReviews { get; set; }
        public List<TopRatedProductDto> TopRatedProducts { get; set; }
    }
}
