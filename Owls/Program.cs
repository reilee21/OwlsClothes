using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Owls;
using Owls.Data;
using Owls.Models;

var builder = WebApplication.CreateBuilder(args).RegisteredServices();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<OwlStoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Db"));
});

builder.Services.AddDbContext<OwlsIdentityContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Db"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddIdentity<OwlsUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                                                    .AddEntityFrameworkStores<OwlsIdentityContext>()
                                                    .AddDefaultTokenProviders(); ;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

await AccountSeedData.Init(app);

app.Run();
