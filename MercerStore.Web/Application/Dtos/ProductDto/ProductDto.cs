namespace MercerStore.Web.Application.Dtos.ProductDto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string MainImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public decimal? DiscountedPrice { get; set; }
    }
}