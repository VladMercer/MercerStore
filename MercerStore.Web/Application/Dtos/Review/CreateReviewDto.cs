namespace MercerStore.Web.Application.Dtos.Review;

public class CreateReviewDto
{
    public int ProductId { get; set; }
    public string? ReviewText { get; set; }
    public int Value { get; set; }
}
