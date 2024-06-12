using Microsoft.AspNetCore.Mvc;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        [Route("/Admin/Profile")]
        public IActionResult Index()
        {
            ViewBag.Nav = "Profile";
            return View();
        }
    }
}
