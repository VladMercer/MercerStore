using MercerStore.Models;

namespace MercerStore.Dtos.ReviewDto
{
    public class ReviewDto
    {
     
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string? ReviewText { get; set; }
        public int Value { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
}
