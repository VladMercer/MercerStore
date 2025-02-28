using MercerStore.Models.Products;

namespace MercerStore.Models.Products
{
    public class ProductStatus
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ProductStatuses Status { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsHit { get; set; }
        public int InStock { get; set; }
        public bool IsUnassigned { get; set; } = true;
        public bool IsLowStock => InStock < 3;
    }
}
