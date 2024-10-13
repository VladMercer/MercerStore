using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers.Api
{
    [Route("/[controller]")]
    [ApiController]
    public class SearchApiController : ControllerBase
    {
        private readonly IElasticSearchService _elasticSearchService;

        public SearchApiController(IElasticSearchService elasticSearchService)
        {
            _elasticSearchService = elasticSearchService;
        }
        [HttpGet]
        public async Task<IActionResult> Search(string query, string? sortOrder, int? pageNumber, int? pageSize)
        {

            var products = await _elasticSearchService.SearchProductsAsync(query);

            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
            }
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
