namespace MercerStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? CategoryImgUrl { get; set; }
        public List<Brand>? Brands { get; set; }
    }
}
