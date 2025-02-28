namespace MercerStore.Dtos.ReviewDto
{
    public class AdminReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string? ReviewText { get; set; }
        public int Value { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public  DateTime EditDate { get; set; }
        public string productImgUrl { get; set; }
    }
}
