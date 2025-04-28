using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface ISkuService
{
    string GenerateSku(Product product);
}