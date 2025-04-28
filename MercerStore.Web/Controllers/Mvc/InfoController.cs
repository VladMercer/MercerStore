using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc;

public class InfoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
