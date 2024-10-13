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
	}
}
