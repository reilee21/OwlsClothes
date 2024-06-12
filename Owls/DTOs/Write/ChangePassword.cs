using System.ComponentModel.DataAnnotations;

namespace Owls.DTOs.Write
{
    public class ChangePassword
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Hãy nhập mật khẩu dài hơn")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không trùng khớp")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string? Token { get; set; }


    }
}
