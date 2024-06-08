using System.ComponentModel.DataAnnotations;

namespace Owls.DTOs
{
	public class RegisterModel
	{
		[Required(ErrorMessage ="Không được để trống")]
		[MinLength(6,ErrorMessage ="Tài khoản không hợp lệ")]
		public string UserName { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Email không hợp lệ")]
		public string Email { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Hãy nhập mật khẩu dài hơn")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [Compare("Password",ErrorMessage ="Mật khẩu nhập lại không trùng khớp")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }

	}
}
