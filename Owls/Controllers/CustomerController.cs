using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Owls.Models;
using System.Security.Claims;

namespace Owls.Controllers
{
    public class CustomerController : Controller
    {

        private readonly UserManager<OwlsUser> _userManager;

        public CustomerController(UserManager<OwlsUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("/Profile")]
        public async Task<IActionResult> Profile()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            OwlsUser u = await _userManager.FindByIdAsync(id);
            return View("Index", u);
        }
    }
}
