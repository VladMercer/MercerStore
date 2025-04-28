using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Carts;

namespace MercerStore.Web.Application.Models.Products;

public class Product : IEntity
{
    public string Name { get; set; }
    public string? SKU { get; set; }
    public string MainImageUrl { get; set; }
    public int CategoryId { get; set; }
    public ProductStatus ProductStatus { get; set; }
    public ProductDescription ProductDescription { get; set; }
    public ProductPricing ProductPricing { get; set; }
    public ICollection<ProductImage>? Images { get; set; }
    public Category Category { get; set; }
    public CartProduct? CartProduct { get; set; }
    public IList<ProductVariant>? ProductVariants { get; set; }
    public IList<Review>? Reviews { get; set; }
    public int Id { get; set; }
}