namespace MercerStore.Web.Application.Models.Products;

public class ProductDescription
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string DescriptionText { get; set; }

    /* public VideoCardDetail? VideoCard { get; set; }
     public ProcessorDetail? Processor { get; set; }
     public MotherboardDetail? Motherboard { get; set; }
     public RamDetail? Ram { get; set; }
     public PowerSupplyDetail? PowerSupply { get; set; }
     public StorageDetail? Storage { get; set; }
     public CaseDetail? Case { get; set; }
     public CoolingSystemDetail? CoolingSystem { get; set; }*/
    public Brand? Brand { get; set; }
}