namespace MercerStore.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoImgUrl { get; set; }
        public List<Product>? Products { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
