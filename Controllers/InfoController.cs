using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
