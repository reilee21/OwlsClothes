using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Owls.Models;

namespace Owls.Views.Shared.Components.SendEmail
{
    public class SendEmailViewComponent : ViewComponent
    {
        private readonly UserManager<OwlsUser> userManager;

        public SendEmailViewComponent(UserManager<OwlsUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cur = User.Identity.Name;
            if (cur != null)
            {
                var user = await userManager.FindByNameAsync(cur);
                if (user != null)
                {
                    ViewBag.UEmail = user.Email;
                }
            }

            return View("Default");
        }
    }
}
