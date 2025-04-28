namespace MercerStore.Web.Application.Models.Products;

public class ProductImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string ProductId { get; set; }
    public Product Product { get; set; }
}