namespace MercerStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Brand>? Brands { get; set; }
    }
}
