namespace MercerStore.Models.DescriptionProducts
{
    public class RamDetail
    {
        public int Id { get; set; }
        // Модель и общие параметры
        public string Model { get; set; }
        public string ManufacturerCode { get; set; }

        // Объем и состав комплекта
        public string MemoryType { get; set; }
        public string MemoryFormFactor { get; set; }
        public int? TotalMemoryVolume { get; set; }
        public int? ModuleMemoryVolume { get; set; }
        public int? ModuleCount { get; set; }
        public bool? RegisteredMemory { get; set; }
        public bool? ECCMemory { get; set; }
        public string RankType { get; set; }

        // Быстродействие
        public string Frequency { get; set; }
        public string XMPProfiles { get; set; }

        // Тайминги
        public int? CASLatency { get; set; }
        public int? RASToCASDelay { get; set; }
        public int? RowPrechargeDelay { get; set; }
        public int? ActivateToPrechargeDelay { get; set; }

        // Конструкция
        public bool? RadiatorPresence { get; set; }
        public string? RadiatorColor { get; set; }
        public string? Height { get; set; }
        public bool? LowProfile { get; set; }

        // Дополнительно
        public string? Voltage { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
