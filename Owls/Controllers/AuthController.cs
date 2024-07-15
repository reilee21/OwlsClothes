using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Owls.DTOs;
using Owls.DTOs.Write;
using Owls.Helper;
using Owls.Models;

namespace Owls.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<OwlsUser> _userManager;
        private readonly SignInManager<OwlsUser> _signInManager;

        public AuthController(UserManager<OwlsUser> userManager, SignInManager<OwlsUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "- Đăng nhập";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Title = "- Đăng ký";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản đã tạm khoá. Xin vui lòng thử lại sau.");
                    return View(user);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Sai tài khoản hoặc mật khẩu.");
                    return View(user);
                }
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new OwlsUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    Address = "",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UpdateProfile(UserProfile profile)
        {
            LocationService location = new LocationService();
            await location.InitializeAsync();
            profile.Ward = location.GetWardName(profile.City, profile.Dicstrict, profile.Ward);
            profile.Dicstrict = location.GetDistrictName(profile.City, profile.Dicstrict);
            profile.City = location.GetCityName(profile.City);

            OwlsUser u = await _userManager.FindByIdAsync(profile.Id);
            if (u == null)
            {
                return NotFound("User not found");
            }
            u.FullName = profile.FullName;
            u.PhoneNumber = profile.PhoneNumber;
            u.City = profile.City;
            u.Dicstrict = profile.Dicstrict;
            u.Ward = profile.Ward;
            u.Address = profile.Address;
            var rs = await _userManager.UpdateAsync(u);
            if (!rs.Succeeded)
            {
                return BadRequest(rs.Errors);
            }
            return Ok(rs);
        }

    }
}
