using Microsoft.AspNetCore.Mvc;

namespace Owls.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Nav = "Orders";

            return View();
        }
    }
}
