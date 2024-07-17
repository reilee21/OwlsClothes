using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Owls.DTOs;
using Owls.DTOs.Write;
using Owls.Helper;
using Owls.Models;

namespace Owls.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class AccountsController : Controller
    {
        private readonly OwlStoreContext owlStoreContext;
        private readonly UserManager<OwlsUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IMapper mapper;
        private readonly int ItemPerPage = 10;

        public AccountsController(OwlStoreContext owlStoreContext, UserManager<OwlsUser> userManager, RoleManager<IdentityRole> roleManager, IMapper map)
        {
            this.owlStoreContext = owlStoreContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = map;
        }
        public async Task<IActionResult> Index(string filter, string? search, int? page = 1)
        {
            ViewBag.Nav = "Accounts";

            var users = string.IsNullOrEmpty(search)
                        ? userManager.Users.ToList()
                        : userManager.Users.Where(u => u.FullName.ToLower().Contains(search.ToLower())).ToList();

            List<OwlsUser> filteredUsers;

            if (filter == "Staff")
            {
                filteredUsers = new List<OwlsUser>();

                foreach (var user in users)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Contains("admin") || roles.Contains("staff"))
                    {
                        filteredUsers.Add(user);
                    }
                }
            }
            else if (filter == "Customer")
            {
                filteredUsers = new List<OwlsUser>();

                foreach (var user in users)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Count == 0)
                    {
                        filteredUsers.Add(user);
                    }
                }
            }
            else
            {
                filteredUsers = users;
            }
            ViewBag.Roles = new SelectList(new List<SelectListItem>
                            {
                                new SelectListItem { Text = "Tất cả", Value = "All"},
                                new SelectListItem { Text = "Nhân viên", Value = "Staff"},
                                new SelectListItem { Text = "Khách hàng", Value = "Customer"},
                            }, "Value", "Text");
            ViewBag.Selected = filter;

            ViewBag.Pager = new Pager()
            {
                CurrentPage = (int)page,
                ItemsPerPage = ItemPerPage,
                TotalItems = filteredUsers.Count,
            };
            ViewBag.Search = search;
            var rs = filteredUsers.Skip(((int)page - 1) * ItemPerPage).Take(ItemPerPage);

            return View(rs);
        }

        [HttpPost]
        public async Task<IActionResult> AddStaff(AccountWrite account)
        {
            var c = await userManager.FindByEmailAsync(account.Email);
            var a = await userManager.FindByNameAsync(account.UserName);
            if (c != null || a != null)
            {
                return BadRequest("Email hoặc tài khoản đã đăng ký");
            }
            if (ModelState.IsValid)
            {
                LocationService location = new LocationService();
                await location.InitializeAsync();
                account.Ward = location.GetWardName(account.City, account.Dicstrict, account.Ward);
                account.Dicstrict = location.GetDistrictName(account.City, account.Dicstrict);
                account.City = location.GetCityName(account.City);
                var newuser = mapper.Map<OwlsUser>(account);
                var create = await userManager.CreateAsync(newuser, account.Password);
                if (create.Succeeded && account.Role != "invalid")
                {
                    await userManager.AddToRoleAsync(newuser, account.Role);
                }
                return Ok();
            }
            return BadRequest("Thông tin không hợp lệ");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccount(string id, AccountWrite account)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            { return NotFound(); }
            var r = await userManager.GetRolesAsync(user);
            account.Password = id;
            if (ModelState.IsValid)
            {
                if (r != null && !r.Contains(account.Role))
                {
                    await userManager.RemoveFromRolesAsync(user, r);
                    if (account.Role != "invalid")
                    {
                        await userManager.AddToRoleAsync(user, account.Role);
                    }
                }

                LocationService location = new LocationService();
                await location.InitializeAsync();
                account.Ward = location.GetWardName(account.City, account.Dicstrict, account.Ward);
                account.Dicstrict = location.GetDistrictName(account.City, account.Dicstrict);
                account.City = location.GetCityName(account.City);
                user.FullName = account.FullName;
                user.PhoneNumber = account.PhoneNumber;
                user.Address = account.Address;
                user.Dicstrict = account.Dicstrict;
                user.Ward = account.Ward;
                user.City = account.City;

                await userManager.UpdateAsync(user);

                return Ok(user);
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetAccount(string id)
        {
            var u = await userManager.FindByIdAsync(id);
            if (u == null)
            { return NotFound(); }
            var roles = await userManager.GetRolesAsync(u);

            LocationService location = new LocationService();
            await location.InitializeAsync();
            string city = u.City;
            u.Ward = location.GetWardId(city, u.Dicstrict, u.Ward);
            u.Dicstrict = location.GetDistrictId(city, u.Dicstrict);

            u.City = location.GetCityId(city);

            return Ok(new
            {
                acc = u,
                role = roles.Any() ? roles[0] : null,
            });
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {


            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest();
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, model.Password);


            return Ok("Password changed successfully.");
        }

    }
}
