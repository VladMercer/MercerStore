using MercerStore.Web.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var homePageViewModel = await _homeService.GetHomePageProduct();
            return View(homePageViewModel);
        }
    }
}
