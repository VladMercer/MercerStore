using MercerStore.Models;

namespace MercerStore.Interfaces
{
    public interface ISKUService
    {
        string GenerateSKU(Product product);
    }
}
