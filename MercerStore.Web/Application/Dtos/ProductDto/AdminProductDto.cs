namespace MercerStore.Web.Application.Dtos.ProductDto
{
    public class AdminProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string MainImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string Status { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public DateTime? DiscountEnd { get; set; }
        public DateTime? DiscountStart { get; set; }
        public int? InStock { get; set; }
        public int? RemainingDiscountDays { get; set; }
    }
}
