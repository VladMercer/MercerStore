using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc;

public class AboutUsController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}