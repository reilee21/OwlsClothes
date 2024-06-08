using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Owls.Models
{
	public class OwlsIdentityContext:IdentityDbContext<OwlsUser>
	{
		public OwlsIdentityContext(DbContextOptions<OwlsIdentityContext> options):base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<IdentityRole>().HasData(
			   new IdentityRole { Name = "admin", NormalizedName = "ADMIN" },
			   new IdentityRole { Name = "staff", NormalizedName = "STAFF" });
			
		}
	}
}
