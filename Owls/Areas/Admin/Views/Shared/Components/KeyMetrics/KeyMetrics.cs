using Microsoft.AspNetCore.Mvc;
using Owls.DTOs;
using Owls.Repositories.ManageRepos;

namespace Owls.Areas.Admin.Views.Shared.Components.KeyMetrics
{
    public class KeyMetricsViewComponent : ViewComponent
    {
        private readonly IManageRepos repos;

        public KeyMetricsViewComponent(IManageRepos manage)
        {
            repos = manage;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Metrics rs = new Metrics();
            DateTime today = DateTime.Today;
            DateTime bf = new DateTime(2000, 1, 1);
            rs.ToDayRevenue = await repos.GetRevenueByTime(today, today);
            rs.TotalRevenue = await repos.GetRevenueByTime(bf, today);
            rs.TotalCustomer = await repos.GetTotalCustomer();
            rs.TotalOrder = repos.GetTotalOrder();
            return View(rs);
        }

    }
}
