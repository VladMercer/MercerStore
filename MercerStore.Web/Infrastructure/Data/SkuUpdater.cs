using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Infrastructure.Data;

public class SkuUpdater : ISkuUpdater
{
    private readonly AppDbContext _context;
    private readonly ISkuService _skuService;

    public SkuUpdater(AppDbContext context, ISkuService skuService)
    {
        _context = context;
        _skuService = skuService;
    }

    public void UpdateSkus()
    {
        var products = _context.Products.Where(p => p.SKU == null).ToList();
        if (products.Count == 0)
        {
            Console.WriteLine("Продуктов с null SKUs не найдено");
        }
        else
        {
            foreach (var product in products)
            {
                product.SKU = _skuService.GenerateSku(product);
                Console.WriteLine($"Product {product.Id} обновлено с SKU {product.SKU}");
            }

            _context.SaveChanges();
            Console.WriteLine("SKUs обновление успешно.");
        }
    }
}
