namespace MercerStore.Models.Products
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public string? Color { get; set; }
        public string? Name { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
