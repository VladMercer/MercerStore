using Microsoft.AspNetCore.Mvc;
using Nest;

namespace MercerStore.Web.Controllers.Mvc
{
    public class AboutUsController : Controller
    {
        private readonly IElasticClient _elasticClient;

        public AboutUsController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
