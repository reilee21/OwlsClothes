using Microsoft.AspNetCore.Identity;
using Owls.Models;
namespace Owls.Data
{
    public static class AccountSeedData
    {
        public static async Task Init(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<OwlsUser>>();


                var adminEmail = "admin@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var admin = new OwlsUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = "admin",
                        Address = "123 SVH",
                        EmailConfirmed = true
                    };
                    string adminPassword = "admin@123";
                    var createAdmin = await userManager.CreateAsync(admin, adminPassword);
                    if (createAdmin.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "admin");
                    }

                }
                var staffEmail = "staff@gmail.com";
                var staffUser = await userManager.FindByEmailAsync(staffEmail);
                if (staffUser == null)
                {
                    var staff = new OwlsUser
                    {
                        UserName = staffEmail,
                        Email = staffEmail,
                        FullName = "staff",
                        Address = "123 SVH",
                        EmailConfirmed = true
                    };
                    string staffPassword = "staff@123";
                    var createStaff = await userManager.CreateAsync(staff, staffPassword);
                    if (createStaff.Succeeded)
                    {
                        await userManager.AddToRoleAsync(staff, "staff");
                    }
                }
            }
        }
    }
}
