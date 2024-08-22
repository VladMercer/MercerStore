using MercerStore.Interfaces;
using MercerStore.Models;

public class SKUService : ISKUService
{
    public string GenerateSKU(Product product)
    {
        return $"{product.CategoryId}{product.Id}";
    }
}