using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.Extentions;
using BP_215UniqloMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<UniqloDbContext>();

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseUserSeed();

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
