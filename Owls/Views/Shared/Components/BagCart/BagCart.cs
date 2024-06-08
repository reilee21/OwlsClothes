using Microsoft.AspNetCore.Mvc;
using Owls.DTOs;
using Owls.Helper;

namespace Owls.Views.Shared.Components.BagCart
{
    public class BagCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = GetCart();
            ViewBag.BagCart = cart.Sum(c=>c.Quantity);
            return View("Default");
        }

        private List<CartItem> GetCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("CartSession");
            return cart ?? new List<CartItem>();
        }
    }
}
