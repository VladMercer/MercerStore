
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers
{
    public class SearchController : Controller
    {


        public SearchController() { }

        public IActionResult Index()
        {
            return View();
        }
    }
}
