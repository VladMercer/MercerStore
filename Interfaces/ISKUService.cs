using MercerStore.Models.Products;
using MercerStore.Models.Products;

namespace MercerStore.Interfaces
{
    public interface ISKUService
    {
        string GenerateSKU(Product product);
    }
}
