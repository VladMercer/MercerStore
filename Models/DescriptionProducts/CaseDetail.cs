namespace MercerStore.Models.DescriptionProducts
{
    public class CaseDetail
    {
        public int Id { get; set; }
        // Модель и общие параметры
        public string Model { get; set; }
        public string ManufacturerCode { get; set; }

        // Форм-фактор и габариты
        public string CaseType { get; set; }
        public string? MotherboardOrientation { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public double Weight { get; set; }

        // Внешний вид
        public string PrimaryColor { get; set; }
        public string Material { get; set; }
        public string MetalThickness { get; set; }
        public bool SidePanelWindow { get; set; }
        public string? WindowMaterial { get; set; }
        public string? FrontPanelMaterial { get; set; }
        public string? LightingType { get; set; }
        public string? LightingColor { get; set; }
        public string? LightingSource { get; set; }
        public string? LightingConnector { get; set; }
        public string? LightingControl { get; set; }

        // Совместимость
        public string? CompatibleMotherboardFormFactors { get; set; }
        public string? CompatiblePowerSupplyFormFactors { get; set; }
        public string? PowerSupplyPlacement { get; set; }
        public string? MaxPowerSupplyLength { get; set; }
        public int HorizontalExpansionSlotsCount { get; set; }
        public int VerticalExpansionSlotsCount { get; set; }
        public string? MaxGpuLength { get; set; }
        public string? MaxCpuCoolerHeight { get; set; }
        public int Internal2_5DriveBaysCount { get; set; }
        public int Internal3_5DriveBaysCount { get; set; }
        public int External3_5DriveBaysCount { get; set; }
        public int DriveBays5_25Count { get; set; }

        // Охлаждение
        public string? IncludedFans { get; set; }
        public string? FrontFanSupport { get; set; }
        public string? RearFanSupport { get; set; }
        public string? TopFanSupport { get; set; }
        public string? BottomFanSupport { get; set; }
        public bool LiquidCoolingSupport { get; set; }
        public string? FrontRadiatorSizes { get; set; }
        public string? TopRadiatorSizes { get; set; }
        public string? RearRadiatorSizes { get; set; }

        // Разъемы и интерфейсы лицевой панели
        public string? IOPanelLocation { get; set; }
        public string? IOConnectors { get; set; }
        public bool BuiltInCardReader { get; set; }

        // Обслуживание
        public bool SidePanelFixationScrews { get; set; }
        public bool CpuCoolerCutout { get; set; }
        public bool CableManagementBehindMotherboardTray { get; set; }
        public bool DustFilters { get; set; }

        // Дополнительно
        public bool BuiltInPowerSupply { get; set; }
        public bool LowNoiseAntiVibrationCases { get; set; }

        // Комплектация
        public string? Accessories { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
