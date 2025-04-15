using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Models.Products;

public class SKUService : ISkuService
{
    public string GenerateSku(Product product)
    {
        var creationDate = DateTime.UtcNow.ToString("yyMM");
        var uniqueSegment = Guid.NewGuid().ToString().Substring(0,6);
       
        return $"{product.CategoryId}-{product.Id}-{creationDate}-{uniqueSegment}";
    }

}