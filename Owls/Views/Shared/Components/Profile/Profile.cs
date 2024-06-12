using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Owls.Helper;
using Owls.Models;
using System.Security.Claims;

namespace Owls.Views.Shared.Components.Profile
{
    public class ProfileViewComponent : ViewComponent
    {
        private readonly UserManager<OwlsUser> userManager;

        public ProfileViewComponent(UserManager<OwlsUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    LocationService location = new LocationService();
                    await location.InitializeAsync();
                    user.Ward = location.GetWardId(user.City, user.Dicstrict, user.Ward);
                    user.Dicstrict = location.GetDistrictId(user.City, user.Dicstrict);
                    user.City = location.GetCityId(user.City);

                    return View(user);
                }
            }
            return View(null);
        }
    }
}
