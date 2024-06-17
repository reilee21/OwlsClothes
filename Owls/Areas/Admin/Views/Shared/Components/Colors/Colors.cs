using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Owls.Models;

namespace Owls.Views.Shared.Components.Colors
{
    public class ColorsViewComponent : ViewComponent
    {
        private readonly OwlStoreContext _storeContext;

        public ColorsViewComponent(OwlStoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rs = await _storeContext.Colors.ToListAsync();
            return View("Default", rs);
        }

    }
}
