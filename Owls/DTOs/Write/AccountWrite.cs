using System.ComponentModel.DataAnnotations;

namespace Owls.DTOs.Write
{
    public class AccountWrite
    {
        [Required(ErrorMessage = "Không được để trống")]
        [MinLength(6, ErrorMessage = "Tài khoản không hợp lệ")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Hãy nhập mật khẩu dài hơn")]
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public string? City { get; set; }
        public string? Dicstrict { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
    }
}
