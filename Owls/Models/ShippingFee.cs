using System.ComponentModel.DataAnnotations;

namespace Owls.Models
{
	public class ShippingFee
	{
		[Key]
		public int Id { get; set; }
		public string City { get; set; }
		public int Fee { get; set; }
	}
}
