using MercerStore.Models.DescriptionProducts;

namespace MercerStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string MainImageUrl { get; set; }
        public ICollection<ProductImage>? Images { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }
        //Описание
        public VideoCardDetail? VideoCard { get; set; }
        public ProcessorDetail? Processor { get; set; }
        public MotherboardDetail? Motherboard { get; set; }
        public RamDetail? Ram { get; set; }
        public PowerSupplyDetail? PowerSupply { get; set; }
        public StorageDetail? Storage { get; set; }
        public CaseDetail? Case { get; set; }
        public CoolingSystemDetail? coolingSystem { get; set; }
        //Описание
        public List<ProductVariant>? ProductVariants { get; set; }
        public List<Rating>? Ratings { get; set; }

    }
}
