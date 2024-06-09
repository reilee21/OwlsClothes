using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Owls.Models;

namespace Owls.Controllers
{
    public class CustomerController : Controller
    {

        private readonly UserManager<OwlsUser> _userManager;

        public CustomerController(UserManager<OwlsUser> userManager)
        {
            _userManager = userManager;
        }


    }
}
