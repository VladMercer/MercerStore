namespace MercerStore.Web.Application.Models.Products.DescriptionProducts;

public class StorageDetail
{
    public int Id { get; set; }

    // Модель и общие параметры
    public string Model { get; set; }
    public string ManufacturerCode { get; set; }

    // Основные характеристики
    public int CapacityGB { get; set; }
    public string FormFactor { get; set; }
    public string PhysicalInterface { get; set; }
    public string M2Key { get; set; }
    public bool NVMe { get; set; }

    // Конфигурация накопителя
    public string? Controller { get; set; }
    public string? BitsPerCell { get; set; }
    public string? MemoryStructure { get; set; }
    public bool? DRAMBuffer { get; set; }
    public int? DRAMBufferSizeMB { get; set; }

    // Показатели скорости
    public string MaxSequentialReadSpeed { get; set; }
    public string MaxSequentialWriteSpeed { get; set; }

    // Надежность
    public string? TBW { get; set; }
    public double? DWPD { get; set; }

    // Дополнительная информация
    public bool? RadiatorIncluded { get; set; }
    public string? PowerConsumption { get; set; }

    // Габариты
    public string? Length { get; set; }
    public string? Width { get; set; }
    public string? Thickness { get; set; }
    public int? WeightGrams { get; set; }
}