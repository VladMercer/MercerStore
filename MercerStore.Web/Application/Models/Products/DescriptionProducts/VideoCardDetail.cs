namespace MercerStore.Web.Application.Models.Products.DescriptionProducts;

public class VideoCardDetail
{
    public int Id { get; set; }

    // Общие параметры
    public string? ManufacturerCode { get; set; }
    public bool IsMiningPurpose { get; set; }
    public bool IsLHR { get; set; }

    // Основные параметры
    public string GPU { get; set; }
    public string Microarchitecture { get; set; }
    public string FabricationProcess { get; set; }

    // Спецификации видеопроцессора
    public int BaseClockMHz { get; set; }
    public int BoostClockMHz { get; set; }
    public int? ALUs { get; set; }
    public int? TextureUnits { get; set; }
    public int? ROPs { get; set; }
    public bool RayTracingSupport { get; set; }
    public int? RayTracingCores { get; set; }
    public int? TensorCores { get; set; }

    // Спецификации видеопамяти
    public int MemorySizeGB { get; set; }
    public string MemoryType { get; set; }
    public int MemoryBusWidth { get; set; }
    public int MemoryBandwidthGBps { get; set; }
    public int MemoryFrequencyMHz { get; set; }

    // Вывод изображения
    public string? DisplayConnectors { get; set; }
    public string? HDMIVersion { get; set; }
    public string? DisplayPortVersion { get; set; }
    public int? MaxMonitors { get; set; }
    public string? MaxResolution { get; set; }

    // Подключение
    public string? Interface { get; set; }
    public string? ConnectionFormFactor { get; set; }
    public int? PCILanes { get; set; }
    public string? PowerConnectors { get; set; }
    public int? RecommendedPSUWattage { get; set; }

    // Система охлаждения
    public string? CoolingType { get; set; }
    public int? FanCount { get; set; }

    // Габариты и вес
    public int? LengthMM { get; set; }
    public int? WidthMM { get; set; }
    public int? ThicknessMM { get; set; }
    public int? WeightGrams { get; set; }

    // Дополнительно
    public bool? IsRGB { get; set; }
    public bool? IsRGBSync { get; set; }
    public bool? HasLCDDisplay { get; set; }
    public bool? HasBIOSSwitch { get; set; }
    public string? DimensionsBracket { get; set; }
}
