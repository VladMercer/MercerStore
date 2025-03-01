using MercerStore.Interfaces;
using MercerStore.Models.Products;
using MercerStore.Models.Products;

public class SKUService : ISKUService
{
    public string GenerateSKU(Product product)
    {
        var creationDate = DateTime.UtcNow.ToString("yyMM");
        var uniqueSegment = Guid.NewGuid().ToString().Substring(0,6);
       
        return $"{product.CategoryId}-{product.Id}-{creationDate}-{uniqueSegment}";
    }

}