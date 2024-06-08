using Microsoft.AspNetCore.Mvc;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Nav = "Dashboard";
            return View();
        }
    }
}
