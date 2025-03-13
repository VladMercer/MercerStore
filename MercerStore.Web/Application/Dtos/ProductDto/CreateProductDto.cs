using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Dtos.ProductDto
{
    public class CreateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public IFormFile? MainImage { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}
