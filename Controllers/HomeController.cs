using MercerStore.Interfaces;
using MercerStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers
{
    
    public class HomeController : Controller
	{

		private readonly IProductRepository _productRepository;

		public HomeController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<IActionResult> Index()
		{

			var products = await _productRepository.GetLastProductsAsync(9);
			var randomProducts = await _productRepository.GetRandomProductsAsync(9);

			var homePageViewModel = new HomePageViewModel
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
					MainImageUrl=r.MainImageUrl,
					Description = r.ProductDescription.DescriptionText,
					Price=r.ProductPricing.OriginalPrice,
					DiscountedPrice = r.ProductPricing.DiscountedPrice
				}).ToList()
			};
			return View(homePageViewModel);
		}


	}
}
