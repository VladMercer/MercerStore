using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Models.Products;

public class Category : IEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? CategoryImgUrl { get; set; }
    public IList<Brand>? Brands { get; set; }
    public int Id { get; set; }
}
