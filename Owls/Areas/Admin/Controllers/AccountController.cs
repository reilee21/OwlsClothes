using Microsoft.AspNetCore.Mvc;

namespace Owls.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Nav = "Account";
            return View();
        }
    }
}
