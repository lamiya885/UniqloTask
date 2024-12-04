using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.ViewModels.Auths;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BP_215UniqloMVC.Controllers
{
    public class AccoountController(UserManager<User> _userManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View();
            User user = new User
            {
                Email = vm.Email,
                FullName = vm.FullName,
                UserName = vm.UserName,
            };


           var result= await _userManager.CreateAsync(user,vm.Password);
            if(!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                        return View();
            }

                        return View();
        }
    }
}
