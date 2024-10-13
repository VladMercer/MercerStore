using MercerStore.Interfaces;
using MercerStore.ViewModels;
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
				Products = products.Select(p => new ProductViewModel
				{
					Id = p.Id,
					Name = p.Name,
					MainImageUrl = p.MainImageUrl,
					Descripton = p.Description,
					Price = p.Price
				}).ToList(),
				RandomProducts = randomProducts.Select(r => new ProductViewModel
				{
					Id = r.Id,
					Name = r.Name,
					MainImageUrl=r.MainImageUrl,
					Descripton=r.Description,
					Price=r.Price
				}).ToList()
			};
			return View(homePageViewModel);
		}


	}
}
