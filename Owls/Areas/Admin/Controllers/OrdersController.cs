﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Owls.DTOs;
using Owls.Helper;
using Owls.Models;
using Owls.Repositories.ColorRepos;
using Owls.Services.Firebase;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrStaff")]

    public class OrdersController : Controller
    {
        private readonly OwlStoreContext _storeContext;
        private readonly IFirebaseStorage firebase;
        private readonly IColorRepos colorRepos;
        public OrdersController(OwlStoreContext storeContext, IFirebaseStorage firebaseStorage, IColorRepos color)
        {
            _storeContext = storeContext;
            firebase = firebaseStorage;
            colorRepos = color;
        }

        public async Task<IActionResult> Index(string? search, int? page, DateTime? from, DateTime? to)
        {
            ViewBag.Nav = "Orders";
            var qr = _storeContext.Orders.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                string value = search.Trim().ToUpper();
                if (value.Length > 1)
                {
                    qr = qr.Where(o => o.OrderId.ToUpper() == value
                                                || o.CustomerName.ToUpper().Contains(value)
                                                || o.CustomerPhone.Equals(value));
                    ViewBag.Search = search;

                }

            }
            if (from != null && to != null && DateTime.Compare((DateTime)from, (DateTime)to) > 0)
            {
                DateTime temp = (DateTime)from;
                from = to;
                to = temp;
            }
            if (from != null)
            {
                qr = qr.Where(o => o.CreateAt >= from);
            }
            if (to != null)
            {
                DateTime d = (DateTime)to;
                qr = qr.Where(o => o.CreateAt < d.AddDays(1));
            }

            int pageNumber = page ?? 1;
            int pageSize = 10;
            var pagedOrders = await qr
                            .OrderByDescending(o => o.CreateAt)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
            int totalItems = await qr.CountAsync();
            Pager pager = new Pager()
            {
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                ItemsPerPage = pageSize,
            };
            ViewBag.Pager = pager;
            ViewBag.From = from;
            ViewBag.To = to;
            return View(pagedOrders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string orderId)
        {
            ViewBag.Nav = "Orders";

            var rs = await _storeContext.Orders
                .Include(o => o.Voucher)
                .Include(o => o.OrderDetails).ThenInclude(od => od.ProductVariant).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (rs == null)
                return RedirectToAction("Index");
            ViewBag.Status = new SelectList(OrderStatus.Status, "Value", "Value");

            foreach (var d in rs.OrderDetails)
            {
                string i = d.ProductVariant.Product.Imagethumbnail;
                if (i.Contains("firebase"))
                    continue;
                d.ProductVariant.Product.Imagethumbnail = await firebase.GetImageStreamAsync(i);
            }
            ViewBag.Colors = await colorRepos.GetColors();

            return View(rs);
        }
        [HttpPost]
        public async Task<IActionResult> Details(Order o)
        {
            Order order = await _storeContext.Orders.FindAsync(o.OrderId);
            if (order == null)
                return RedirectToAction("Index");
            order.Status = o.Status;
            order.IsPaid = o.IsPaid;
            order.UpdateAt = DateTime.Now;
            _storeContext.Entry(order).State = EntityState.Modified;
            await _storeContext.SaveChangesAsync();
            return RedirectToAction("Details", new { orderId = o.OrderId });
        }

    }
}
