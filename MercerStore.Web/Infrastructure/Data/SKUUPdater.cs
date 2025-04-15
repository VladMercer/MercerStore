using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Infrastructure.Data;

public class SKUUpdater : ISkuUpdater
{
    private readonly AppDbContext _context;
    private readonly ISkuService _skuService;

    public SKUUpdater(AppDbContext context, ISkuService skuService)
    {
        _context = context;
        _skuService = skuService;
    }

    public void UpdateSKUs()
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