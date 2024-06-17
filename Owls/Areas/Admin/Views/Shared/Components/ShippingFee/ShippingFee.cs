using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Owls.Models;

namespace Owls.Views.Shared.Components.ShippingFee
{
    public class ShippingFeeViewComponent : ViewComponent
    {
        private readonly OwlStoreContext _storeContext;

        public ShippingFeeViewComponent(OwlStoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rs = await _storeContext.ShippingFees.ToListAsync();
            return View("Default", rs);
        }
    }
}
