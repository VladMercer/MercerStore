namespace MercerStore.Web.Application.Models.Products;

public class ProductPricing
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal? FixedDiscountPrice { get; set; }
    public decimal? DiscountPercentage { get; set; }

    public decimal? DiscountedPrice
    {
        get
        {
            if (DiscountEnd.HasValue && DiscountEnd.Value < DateTime.UtcNow) return null;

            if (FixedDiscountPrice.HasValue) return FixedDiscountPrice.Value;
            if (DiscountPercentage.HasValue) return OriginalPrice * (1 - DiscountPercentage.Value / 100);
            return null;
        }
    }

    public DateTime? DiscountStart { get; set; }
    public DateTime? DiscountEnd { get; set; }
    public DateTime? DateOfPriceChange { get; set; }

    public int? RemainingDiscountDays
    {
        get
        {
            if (DiscountEnd.HasValue)
            {
                var remainingDays = (DiscountEnd.Value - DateTime.UtcNow).Days;
                return remainingDays > 0 ? remainingDays : null;
            }

            return null;
        }
    }
}