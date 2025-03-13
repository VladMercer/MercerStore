using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Products;

namespace MercerStore.Web.Application.Services
{
    public class HomeService : IHomeService
    {
        private readonly IProductRepository _productRepository;

        public HomeService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<HomePageViewModel> GetHomePageProduct()
        {
            var products = await _productRepository.GetLastProductsAsync(9);
            var randomProducts = await _productRepository.GetRandomProductsAsync(9);

            return new HomePageViewModel
            {
                Products = products.Select(p => new RandomProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    MainImageUrl = p.MainImageUrl,
                    Description = p.ProductDescription.DescriptionText,
                    Price = p.ProductPricing.OriginalPrice,
                    DiscountedPrice = p.ProductPricing.DiscountedPrice
                }).ToList(),
                RandomProducts = randomProducts.Select(r => new RandomProductViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    MainImageUrl = r.MainImageUrl,
                    Description = r.ProductDescription.DescriptionText,
                    Price = r.ProductPricing.OriginalPrice,
                    DiscountedPrice = r.ProductPricing.DiscountedPrice
                }).ToList()
            };
        }
    }
}
