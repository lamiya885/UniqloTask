using System.Configuration;
using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.Extentions;
using BP_215UniqloMVC.Helpers;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.Services.Abstract;
using BP_215UniqloMVC.Services.Implements;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BP_215UniqloMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
             builder.Services.AddDbContext<UniqloDbContext>(opt=>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("MsSQL"));

            });
            builder.Services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.AllowedUserNameCharacters =
                "qwertyuiopasdfghjklzxcvbnm0123456789._";
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<UniqloDbContext>();


           

            builder.Services.AddScoped<IEmailService, EmailService>();
            var opt = new SmtpOptions();
            builder.Configuration.GetSection(SmtpOptions.Name).Bind(opt);
            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.Name));
            builder.Configuration.GetSection(SmtpOptions.Name).Get<SmtpOptions>();
            builder.Services.Configure<DataProtectionTokenProviderOptions>(optionns => optionns.TokenLifespan = TimeSpan.FromHours(1));
            builder.Services.ConfigureApplicationCookie(x =>
            {
                x.LoginPath = "/login";
                x.AccessDeniedPath = "/Home/AccessDenied";
            });

            builder.Services.AddSession();

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseUserSeed();
            app.UseSession();

            app.MapControllerRoute(
                name: "register",
                pattern: "register",
                defaults: new { controller = "Account", action = "Register" });


            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");


            app.MapControllerRoute(
                name:"default",
                pattern:"{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
