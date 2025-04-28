using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc;

public class AboutUsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
