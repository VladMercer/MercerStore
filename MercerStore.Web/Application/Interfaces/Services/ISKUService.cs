using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Interfaces
{
    public interface ISKUService
    {
        string GenerateSKU(Product product);
    }
}
