using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers
{
	public class SearchController : Controller
	{
		private readonly IElasticSearchService _elasticSearchService;

		public SearchController(IElasticSearchService elasticSearchService)
		{
			_elasticSearchService = elasticSearchService;
		}

		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Search(string query)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return View(new List<SearchProductVIewModel>()); 
			}
			var products = await _elasticSearchService.SearchProductsAsync(query);

			var productViewModels = products.Select(p => new SearchProductVIewModel
			{
				Id = p.Id,
				Name = p.Name,
				Description = p.Description,
				Price = p.Price,
				ImageUrl = p.MainImageUrl,
				CategoryId = p.CategoryId
			}).ToList();

			return View(productViewModels);
		}
	}
}
