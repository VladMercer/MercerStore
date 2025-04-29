using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Infrastructure.Services;

public class SkuService : ISkuService
{
    public string GenerateSku(Product product)
    {
        var creationDate = DateTime.UtcNow.ToString("yyMM");
        var uniqueSegment = Guid.NewGuid().ToString().Substring(0, 6);

        return $"{product.CategoryId}-{product.Id}-{creationDate}-{uniqueSegment}";
    }
}
