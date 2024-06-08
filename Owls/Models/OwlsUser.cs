using Microsoft.AspNetCore.Identity;

namespace Owls.Models
{
	public class OwlsUser : IdentityUser
	{
		public string FullName { get; set; }
		public string? City { get; set; }
		public string? Dicstrict { get; set; }
		public string? Ward { get; set; }
		public string? Address { get; set; }
	}
}
