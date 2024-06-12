using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
