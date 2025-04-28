namespace MercerStore.Web.Application.Models.Products.DescriptionProducts;

public class ProcessorDetail
{
    public int Id { get; set; }

    // Общие параметры
    public string Model { get; set; }
    public string Socket { get; set; }
    public string ManufacturerCode { get; set; }
    public int ReleaseYear { get; set; }
    public bool CoolingSystemIncluded { get; set; }
    public bool ThermalInterfaceIncluded { get; set; }

    // Ядро и архитектура
    public int TotalCores { get; set; }
    public int PerformanceCores { get; set; }
    public int EnergyEfficientCores { get; set; }
    public int MaxThreads { get; set; }
    public string L2Cache { get; set; }
    public string L3Cache { get; set; }
    public string TechnologyProcess { get; set; }
    public string Core { get; set; }

    // Частота и возможность разгона
    public string BaseFrequency { get; set; }
    public string MaxTurboFrequency { get; set; }

    // Параметры оперативной памяти
    public string? MemoryType { get; set; }
    public string? MaxSupportedMemory { get; set; }
    public int? MemoryChannels { get; set; }
    public string? MemoryFrequency { get; set; }
    public bool? ECCSupport { get; set; }

    // Тепловые характеристики
    public int TDP { get; set; }
    public int MaxTemperature { get; set; }

    // Графическое ядро
    public bool? IntegratedGraphics { get; set; }

    // Шина и контроллеры
    public string? PCIeController { get; set; }
    public int? PCIeLanes { get; set; }
}
