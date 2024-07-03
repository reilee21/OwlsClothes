using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrStaff")]

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
