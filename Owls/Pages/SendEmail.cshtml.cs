using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Owls.Models;
using Owls.Services.Mail;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Owls.Pages
{
    public class SendEmailModel : PageModel
    {
        private readonly UserManager<OwlsUser> _userManager;
        private readonly IEmailService _emailService;

        public SendEmailModel(UserManager<OwlsUser> userManager, IEmailService mail)
        {
            _userManager = userManager;
            _emailService = mail;
        }

        [BindProperty]
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }


        public bool Suceeded { get; set; }
        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                Email = user.Email;
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null) { return Page(); }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackurl = Url.Page("/ChangePass",
                                        pageHandler: null,
                                        values: new { userId = user.Id, code = token },
                                        protocol: Request.Scheme
                );
            string body = $"Nhấp vào đường link để thay đổi mật khẩu: <a href='{callbackurl}'>Cập nhật mật khẩu</a>";
            await _emailService.SendEmailAsync(Email, "Owls Clothes", body);
            Suceeded = true;
            return Page();
        }
    }
}
