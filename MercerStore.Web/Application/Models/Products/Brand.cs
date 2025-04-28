namespace MercerStore.Web.Application.Models.Products;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LogoImgUrl { get; set; }
    public IList<Product>? Products { get; set; }
    public IList<Category>? Categories { get; set; }
}