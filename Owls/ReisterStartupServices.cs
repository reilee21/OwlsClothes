﻿using Microsoft.AspNetCore.Identity;
using Owls.Repositories.ColorRepos;
using Owls.Repositories.ManageRepos;
using Owls.Repositories.OrderRepos;
using Owls.Repositories.ProductRepos;
using Owls.Services.Firebase;
using Owls.Services.Mail;
using Owls.Services.Payment;

namespace Owls
{
    public static class ReisterStartupServices
    {
        public static WebApplicationBuilder RegisteredServices(this WebApplicationBuilder builder)
        {

            builder.Services.AddScoped<IProductRepos, ProductRepos>();
            builder.Services.AddScoped<IProductVariant, ProductVariantsRepos>();

            builder.Services.AddScoped<IColorRepos, ColorRepos>();
            builder.Services.AddScoped<IOrderRepos, OrderRepos>();
            builder.Services.AddScoped<IManageRepos, ManageRepos>();

            builder.Services.AddTransient<IFirebaseStorage, FirebaseStorageService>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IPayment, PaymentSV>();



            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddScoped<HttpContextAccessor>();
            builder.Services.ConfigureApplicationCookie(opt =>
            {
                opt.AccessDeniedPath = "/";
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOrStaff", policy =>
                    policy.RequireRole("admin", "staff"));
            });
            return builder;
        }
    }
}
