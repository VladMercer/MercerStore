using MercerStore.Models.Products;

namespace MercerStore.Models.Products.DescriptionProducts
{
    public class PowerSupplyDetail
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string ManufacturerCode { get; set; }
        public int Wattage { get; set; }

        // Внешний вид
        public string FormFactor { get; set; }
        public string Color { get; set; }
        public bool ModularCables { get; set; }
        public bool BraidedCables { get; set; }
        public string CableColor { get; set; }
        public string LightingType { get; set; }

        // Кабели и разъемы
        public string MainPowerConnector { get; set; }
        public string CPUConnectors { get; set; }
        public string GPUPowerConnectors { get; set; }
        public int SataPowerConnectors { get; set; }
        public int MolexPowerConnectors { get; set; }
        public string MainCableLength { get; set; }
        public string CPUCableLength { get; set; }
        public string GPUCableLength { get; set; }
        public string SataCableLength { get; set; }
        public string MolexCableLength { get; set; }

        // Электрические параметры
        public int PowerOn12VLine { get; set; }
        public int CurrentOn12VLine { get; set; }
        public int CurrentOn3_3VLine { get; set; }
        public int CurrentOn5VLine { get; set; }
        public int StandbyCurrent5V { get; set; }
        public int CurrentOnNegative12VLine { get; set; }
        public string InputVoltageRange { get; set; }

        // Система охлаждения
        public string CoolingSystem { get; set; }
        public string FanSize { get; set; }
        public string FanControl { get; set; }
        public bool HybridMode { get; set; }

        // Сертификация
        public string PlusCertification { get; set; }
        public string PowerFactorCorrection { get; set; }
        public string StandardCompliance { get; set; }
        public string ProtectionTechnologies { get; set; }

        // Дополнительная информация
        public bool PowerCableIncluded { get; set; }
        public string PackageContents { get; set; }
        public string Features { get; set; }

        // Габариты и вес
        public string? Length { get; set; }
        public string? Width { get; set; }
        public string? Height { get; set; }
        public double? Weight { get; set; }

    }
}
