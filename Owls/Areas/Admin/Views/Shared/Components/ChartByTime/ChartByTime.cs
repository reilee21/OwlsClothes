using Microsoft.AspNetCore.Mvc;

namespace Owls.Areas.Admin.Views.Shared.Components.ChartByTime
{
    public class ChartByTimeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
