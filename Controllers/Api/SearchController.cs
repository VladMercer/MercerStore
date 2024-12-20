using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Authorize]
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IElasticSearchService _elasticSearchService;

        public SearchController(IElasticSearchService elasticSearchService)
        {
            _elasticSearchService = elasticSearchService;
        }
        /// <summary>
        /// Найти и отфильтровать продукты.
        /// </summary>
        /// <param name="query">запрос</param>
        /// <param name="sortOrder">Сортировка: "name_desc", "price_asc", "price_desc"</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Количество продуктов на странице</param>
        /// <returns>Список найденных продуктов</returns>
        [HttpGet("search")]
        public async Task<IActionResult> Search(string query, string? sortOrder, int? pageNumber, int? pageSize)
        {

            var products = await _elasticSearchService.SearchProductsAsync(query);

			products = sortOrder switch
			{
				"price_asc" => products.OrderBy(p => p.Price).ToList(),
				"price_desc" => products.OrderByDescending(p => p.Price).ToList(),
				"name_desc" => products.OrderByDescending(p => p.Name).ToList(),
				_ => products.OrderBy(p => p.Name).ToList(),
			};
			var totalItems = products.Count();

            if(pageNumber.HasValue && pageSize.HasValue) 
            { 
                products = products.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
            }
            


            var productViewModels = products.Select(p => new SearchProductVIewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                MainImageUrl = p.MainImageUrl,
                CategoryId = p.CategoryId
            }).ToList();

            return Ok(new
            {
                Products = productViewModels,
                TotalItems = totalItems,
                TotalPages = pageSize.HasValue ? (int)Math.Ceiling((double)totalItems / pageSize.Value) : (int?)null,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }
    }
}
