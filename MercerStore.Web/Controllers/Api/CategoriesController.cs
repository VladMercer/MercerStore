using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace MercerStore.Web.Controllers.Api
{
    [Authorize]
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] CateroryFilterRequest request)
        {
            var result = await _categoryService.GetFilteredProducts(request);
            return Ok(result);
        }

        [HttpGet("price-range/{categoryId}")]
        public async Task<IActionResult> GetPriceRange(int categoryId)
        {
            var priceRange = await _categoryService.GetPriceRange(categoryId);
            return Ok(priceRange);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }
    }
}