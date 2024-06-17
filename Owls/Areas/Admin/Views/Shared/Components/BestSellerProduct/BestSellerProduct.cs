using Microsoft.AspNetCore.Mvc;

namespace Owls.Areas.Admin.Views.Shared.Components.TopSalesProduct
{
    public class BestSellerProductViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
