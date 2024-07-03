using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Owls.DTOs;
using Owls.Models;

namespace Owls.Areas.Admin.Views.Shared.Components.NewOrders
{
    public class NewOrdersViewComponent : ViewComponent
    {
        private readonly OwlStoreContext _storeContext;

        public NewOrdersViewComponent(OwlStoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(int page = 1, int pagesize = 3)
        {
            var qr = _storeContext.Orders.AsQueryable();
            DateTime now = DateTime.Now;
            //DateTime now = new DateTime(2024, 6, 8);
            qr = qr.Where(o => o.CreateAt.Date == now.Date);

            var newOrders = await qr
                           .OrderByDescending(o => o.CreateAt)
                           .Skip((page - 1) * pagesize)
                           .Take(pagesize)
                           .ToListAsync();
            int totalItems = await qr.CountAsync();
            Pager pager = new Pager()
            {
                TotalItems = totalItems,
                CurrentPage = page,
                ItemsPerPage = pagesize,
            };
            ViewBag.Pager = pager;

            return View(newOrders);
        }
    }
}
