using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Owls.DTOs;
using Owls.Helper;
using Owls.Models;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly OwlStoreContext context;

        public HomeController(OwlStoreContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Nav = "Dashboard";
            return View();
        }
        [Route("/api/Demo")]
        public IActionResult Demo()
        {
            return Ok("123");
        }
        [Route("/api/GetRevenueByTime")]
        public async Task<IActionResult> GetRevenue(DateTime from, DateTime to, string range)
        {
            var rvn = context.Orders.Where(o => o.CreateAt >= from && o.CreateAt <= to);

            IEnumerable<RevenueRs> revenues;
            if (range == "day")
            {
                revenues = await rvn
                  .GroupBy(o => o.CreateAt.Date)
                  .Select(g => new RevenueRs
                  {
                      Date = g.Key,
                      Amount = g.Sum(o => (double)o.SubTotal)
                  })
                  .ToListAsync();
            }
            else if (range == "week")
            {
                revenues = rvn
                    .AsEnumerable()
                    .GroupBy(o => new { Year = o.CreateAt.Year, Week = o.CreateAt.GetIso8601WeekOfYear() })
                    .Select(g => new RevenueRs
                    {
                        Date = g.Min(o => o.CreateAt).Date,
                        Amount = g.Sum(o => (double)o.SubTotal)
                    }).ToList();
            }
            else // month
            {
                revenues = await rvn
                    .GroupBy(o => new { o.CreateAt.Year, o.CreateAt.Month })
                    .Select(g => new RevenueRs
                    {
                        Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                        Amount = g.Sum(o => (double)o.SubTotal)
                    })
                    .ToListAsync();
            }

            return Ok(revenues);
        }
        [Route("/api/BestSeller")]

        public async Task<IActionResult> BestSeller(int take = 5, string type = "revenue")
        {
            List<BestSellerProduct> bestSellers;

            if (type.Equals("revenue"))
            {
                bestSellers = await context.OrderDetails.GroupBy(od => new { od.ProductVariant.ProductId, od.ProductVariant.Product.Name })
                                                        .Select(g => new BestSellerProduct
                                                        {
                                                            ProductName = g.Key.Name,
                                                            Revenue = (double)g.Sum(od => od.Quantity * od.Price),
                                                            Quantity = g.Sum(od => od.Quantity.Value)
                                                        })
                                                        .OrderByDescending(bs => bs.Revenue)
                                                        .Take(take)
                                                        .ToListAsync();
            }
            else
            {
                bestSellers = await context.OrderDetails.GroupBy(od => new { od.ProductVariant.Product.ProductId, od.ProductVariant.Product.Name })
                                                        .Select(g => new BestSellerProduct
                                                        {
                                                            ProductName = g.Key.Name,
                                                            Revenue = (double)g.Sum(od => od.Quantity * od.Price),
                                                            Quantity = g.Sum(od => od.Quantity.Value)
                                                        })
                                                        .OrderByDescending(bs => bs.Quantity)
                                                        .Take(take)
                                                        .ToListAsync();

            }
            return Ok(bestSellers);

        }

        public IActionResult LoadOrders(int page = 1, int pageSize = 3)
        {
            return ViewComponent("NewOrders", new { page, pageSize });
        }




    }
}
