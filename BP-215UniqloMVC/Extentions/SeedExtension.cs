using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.Enums;
using BP_215UniqloMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BP_215UniqloMVC.Extentions
{
    public static class SeedExtension
    {
        public static void UseUserSeed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
              var userManager=  scope.ServiceProvider.GetRequiredService<UserManager<User>>();
              var roleManager=  scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if(!roleManager.Roles.Any())
                {
                    foreach(var item in Enum.GetValues(typeof(Roles)))
                    {
                        roleManager.CreateAsync(new IdentityRole(item.ToString())).Wait();
                    }

                }
                if(userManager.Users.Any(x=>x.NormalizedUserName=="ADMIN"))
                {
                    User user = new User
                   { 
                    FullName = "Admin",
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                    ProfileImageUrl = "photo.jpg"
                };
                    userManager.CreateAsync(user, "123").Wait();
                    userManager.AddToRoleAsync(user,nameof(Roles.Admin)).Wait();

                }
            }

        }
    }
}
