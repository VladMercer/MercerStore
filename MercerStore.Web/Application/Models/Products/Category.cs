using MercerStore.Web.Application.Interfaces;

namespace MercerStore.Web.Application.Models.Products
{
    public class Category : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? CategoryImgUrl { get; set; }
        public List<Brand>? Brands { get; set; }
    }
}
