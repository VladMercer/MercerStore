namespace MercerStore.Web.Application.Models.Products.DescriptionProducts
{
    public class MotherboardDetail
    {
        public int Id { get; set; }
        // Модель и общие параметры
        public string Model { get; set; }
        public string Series { get; set; }
        public string Color { get; set; }
        public int ReleaseYear { get; set; }

        // Форм-фактор и размеры
        public string FormFactor { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }

        // Процессор и чипсет
        public string Socket { get; set; }
        public string Chipset { get; set; }
        public string CompatibleIntelCores { get; set; }

        // Память
        public string MemoryType { get; set; }
        public string MemoryFormFactor { get; set; }
        public int MemorySlots { get; set; }
        public int MemoryChannels { get; set; }
        public string MaxMemoryVolume { get; set; }
        public string MaxMemoryFrequency { get; set; }

        // Слоты расширения
        public string PCIeVersion { get; set; }
        public string PCIeSlots { get; set; }
        public bool SLICrossFireSupport { get; set; }
        public int SLICrossFireCards { get; set; }
        public int PCIeX1Slots { get; set; }

        // Контроллеры накопителей
        public bool? NVMeSupport { get; set; }
        public string? NVMePCIeVersion { get; set; }
        public int? M2Slots { get; set; }
        public string? M2PCIeProcessorLines { get; set; }
        public int? SATAPorts { get; set; }
        public bool? SATA_RAIDSupport { get; set; }

        // Порты на задней панели
        public string? USBTypeAPorts { get; set; }
        public bool? USBTypeCPort { get; set; }
        public string? VideoOutputs { get; set; }
        public int? RJ45Ports { get; set; }
        public int? AnalogAudioPorts { get; set; }
        public bool? SPDIFPort { get; set; }

        // Разъемы на плате
        public string? InternalUSBTypeAPorts { get; set; }
        public bool? InternalUSBTypeCPort { get; set; }
        public int? CPUFanPowerConnectors { get; set; }
        public int? CaseFanPowerConnectors4Pin { get; set; }
        public int? CaseFanPowerConnectors3Pin { get; set; }
        public bool? ARGBConnector5V_D_G { get; set; }
        public int? RGBConnector12V_G_R_B { get; set; }
        public bool? WirelessModuleM2 { get; set; }
        public bool? RS232Connector { get; set; }

        // Аудио
        public string? AudioScheme { get; set; }
        public string? AudioChipset { get; set; }

        // Сеть
        public string? NetworkSpeed { get; set; }
        public string? NetworkAdapter { get; set; }
        public bool? WiFiStandard { get; set; }
        public string? BluetoothVersion { get; set; }
        public bool WirelessAdapter { get; set; }

        // Питание и охлаждение
        public string? MainPowerConnector { get; set; }
        public string? CPUPowerConnector { get; set; }
        public int PowerPhaseCount { get; set; }
        public string PassiveCooling { get; set; }
        public bool ActiveCooling { get; set; }

        // Дополнительная информация
        public bool OnBoardButtons { get; set; }
        public bool BoardLighting { get; set; }
        public string? LightingSyncSoftware { get; set; }

        // Комплектация
        public string PackageContents { get; set; }

        // Габариты и вес в упаковке
        public string? BoxLength { get; set; }
        public string? BoxWidth { get; set; }
        public string? BoxHeight { get; set; }
        public string? BoxWeight { get; set; }

    }
}
