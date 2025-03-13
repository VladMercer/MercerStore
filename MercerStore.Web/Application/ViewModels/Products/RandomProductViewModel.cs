namespace MercerStore.Web.Application.ViewModels.Products
{
    public class RandomProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MainImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
    }
}
