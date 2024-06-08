using Owls.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Owls.DTOs
{
	public class CheckOutModel
	{
		[Required]
		public string CutomerName { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		[Required]
		[NotEqualTo("Invalid")]
		public string City { get; set; }
		[Required]
		[NotEqualTo("Invalid")]
		public string Dicstrict { get; set; }
		[Required]
		public string Ward { get; set; }
		[Required]
		public string Address { get; set; }

		public IEnumerable<CartItem>? Carts { get; set; }

	}
}
