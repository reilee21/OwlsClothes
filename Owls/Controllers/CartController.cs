﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Owls.DTOs;
using Owls.Helper;
using Owls.Models;
using Owls.Repositories.ColorRepos;
using Owls.Repositories.OrderRepos;
using Owls.Repositories.ProductRepos;
using Owls.Services.Firebase;
using Owls.Services.Payment;
using System.Security.Claims;

namespace Owls.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "CartSession";
        private readonly IProductRepos productRepos;
        private readonly IProductVariant productVariantRepos;
        private readonly IFirebaseStorage firebaseStorage;
        private readonly IColorRepos colorRepos;
        private readonly OwlStoreContext context;
        private readonly IOrderRepos orderRepos;
        private readonly UserManager<OwlsUser> userManager;
        private readonly IPayment payment;


        public CartController(IProductRepos productRepos, IProductVariant productVariantRepos,
            IFirebaseStorage firebaseStorage, IColorRepos colorRepos, IOrderRepos order, IPayment payment,
            OwlStoreContext context, UserManager<OwlsUser> userManager)
        {
            this.productRepos = productRepos;
            this.productVariantRepos = productVariantRepos;
            this.firebaseStorage = firebaseStorage;
            this.colorRepos = colorRepos;
            this.context = context;
            this.orderRepos = order;
            this.userManager = userManager;
            this.payment = payment;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "- Giỏ hàng";
            var cart = await GetCart();
            await SetImg(cart);
            var colors = await colorRepos.GetColors();
            ViewBag.Colors = colors;
            ViewBag.Total = await GetTotal();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCartItem([FromBody] CartItem item)
        {
            ProductVariant variant = await productVariantRepos.GetVariant(item.Sku);
            if (variant == null || item.Quantity > variant.Quantity)
            {
                return BadRequest(item);
            }
            var cart = await GetCart();

            var cartItem = cart.FirstOrDefault(c => c.Sku == item.Sku);
            if (cartItem == null)
            {
                CartItem newC = await productRepos.GetProductCart(item.Sku, item.Quantity);
                cart.Add(newC);
            }
            else
            {
                cartItem.Quantity += item.Quantity;
            }

            SaveCart(cart);
            return Ok(cart.Sum(c => c.Quantity));

        }
        [HttpPost]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartItem item)
        {
            var cart = await GetCart();
            var cartItem = cart.FirstOrDefault(c => c.Sku == item.Sku);
            if (cartItem == null)
            { return BadRequest(item); }
            ProductVariant variant = await productVariantRepos.GetVariant(item.Sku);
            if (variant == null || item.Quantity > variant.Quantity)
            {
                return BadRequest(item);
            }
            cartItem.Quantity = item.Quantity;
            SaveCart(cart);
            var total = await GetTotal();
            return Ok(new { TotalQuantity = cart.Sum(c => c.Quantity), UpdatedItem = item, Total = total });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCartItem([FromBody] CartItem item)
        {
            var cart = await GetCart();
            var cartItem = cart.FirstOrDefault(c => c.Sku == item.Sku);
            if (cartItem == null)
            { return BadRequest(item); }
            cart.Remove(cartItem);
            SaveCart(cart);
            var total = await GetTotal();
            return Ok(new { TotalQuantity = cart.Sum(c => c.Quantity), Total = total });
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            ViewBag.Title = "- Thanh toán";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Login", "Auth");
            }
            var user = await userManager.FindByIdAsync(userId);
            LocationService locationService = new LocationService();
            await locationService.InitializeAsync();
            string w = locationService.GetWardId(user.City, user.Dicstrict, user.Ward);
            string d = locationService.GetDistrictId(user.City, user.Dicstrict);
            string c = locationService.GetCityId(user.City);

            CheckOutModel checkout = new CheckOutModel()
            {
                CutomerName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                City = c,
                Dicstrict = d,
                Ward = w,
                Address = user.Address
            };

            var cart = await GetCart();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }
            await SetImg(cart);
            var colors = await colorRepos.GetColors();
            var ship = await context.ShippingFees.Select(p => new { p.City, p.Fee }).ToListAsync();
            ViewBag.Colors = colors;
            ViewBag.ShippingFees = ship;
            ViewBag.SubTotal = await GetTotal();
            checkout.Carts = cart;
            return View(checkout);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckOutModel checkOutModel)
        {
            var cart = await GetCart();
            checkOutModel.Carts = cart;
            if (!ModelState.IsValid)
            {
                await SetImg(cart);
                var colors = await colorRepos.GetColors();
                var ship = await context.ShippingFees.Select(p => new { p.City, p.Fee }).ToListAsync();
                ViewBag.Colors = colors;
                ViewBag.ShippingFees = ship;
                ViewBag.SubTotal = await GetTotal();
                return View(checkOutModel);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            LocationService locationService = new LocationService();
            await locationService.InitializeAsync();
            checkOutModel.Ward = locationService.GetWardName(checkOutModel.City, checkOutModel.Dicstrict, checkOutModel.Ward);
            checkOutModel.Dicstrict = locationService.GetDistrictName(checkOutModel.City, checkOutModel.Dicstrict);
            checkOutModel.City = locationService.GetCityName(checkOutModel.City);

            var rs = await orderRepos.CheckOut(cart, checkOutModel, userId);
            if (rs == null)
            { return RedirectToAction("Index"); }
            SaveCart(new List<CartItem>());
            if (rs.PaymentMethod == PaymentMethod.BANK.ToString())
            {
                var paymentlink = await payment.CreatePaymentLink(rs);
                return Redirect(paymentlink.checkoutUrl);
            }

            return View("Success");
        }
        [HttpGet]
        public async Task<IActionResult> ApplyVoucher(string? code)
        {
            if (string.IsNullOrEmpty(code))
                return NotFound();
            string vchcode = code.Trim().ToUpper();
            var rs = await context.Vouchers.FirstOrDefaultAsync(v => v.Name.Equals(vchcode));
            if (rs == null)
                return NotFound();
            if (!rs.IsActive || rs.Quantity < 1)
                return BadRequest("Voucher không còn sử dụng được");
            return Ok(rs);
        }


        private async Task<double> GetTotal()
        {
            var cart = await GetCart();
            double total = 0;
            foreach (var item in cart)
            {
                total += (double)(item.Quantity * item.Price * (1 - item.Discount / 100));
            }
            return total;
        }
        private async Task<List<CartItem>> GetCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey);
            if (cart != null)
            {
                foreach (var item in cart)
                {
                    var pro = await productVariantRepos.GetVariant(item.Sku);
                    if (pro == null)
                        new List<CartItem>();

                    item.Discount = pro.Promotions.Sum(p => p.DiscountPercentage);
                }
            }

            return cart ?? new List<CartItem>();
        }
        private async Task<List<CartItem>> SetImg(List<CartItem> cart)
        {
            if (cart != null)
            {
                foreach (var item in cart)
                {
                    if (item.Image != null)
                    {
                        item.Image = await firebaseStorage.GetImageStreamAsync(item.Image);
                    }
                }
            }
            return cart;
        }
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
    }
}
