namespace MercerStore.Models.DescriptionProducts
{
    public class CoolingSystemDetail
    {
        public int Id { get; set; }
        // Модель и общие параметры
        public string Model { get; set; }
        public string ManufacturerCode { get; set; }
        public string SocketCompatibility { get; set; }
        public int TDP { get; set; }

        // Радиатор
        public string ConstructionType { get; set; }
        public string BaseMaterial { get; set; }
        public string RadiatorMaterial { get; set; }
        public int HeatPipesCount { get; set; }
        public string HeatPipeDiameter { get; set; }
        public bool NickelPlating { get; set; }
        public string RadiatorColor { get; set; }

        // Вентилятор
        public int FansIncludedCount { get; set; }
        public int MaxFansCount { get; set; }
        public string FanDimensions { get; set; }
        public string FanColor { get; set; }
        public string FanConnector { get; set; }
        public int MaxRotationSpeed { get; set; }
        public int MinRotationSpeed { get; set; }
        public string RotationSpeedControl { get; set; }
        public double MaxAirflow { get; set; }
        public double MaxNoiseLevel { get; set; }
        public double RatedCurrent { get; set; }
        public int RatedVoltage { get; set; }
        public string BearingType { get; set; }

        // Дополнительно
        public bool ThermalPasteIncluded { get; set; }
        public string? LightingType { get; set; }

        // Комплектация
        public string? MountingKit { get; set; }

        // Габариты и вес
        public string? Height { get; set; }
        public string? Width { get; set; }
        public string? Length { get; set; }
        public double? Weight { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
