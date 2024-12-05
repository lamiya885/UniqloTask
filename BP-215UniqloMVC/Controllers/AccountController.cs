using BP_215UniqloMVC.Enums;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.ViewModels.Auths;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BP_215UniqloMVC.Controllers
{
    public class AccountController(UserManager<User> _userManager,SignInManager<User> signInManager) : Controller
    {
        
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View();
            User user = new User
            {
                Email = vm.Email,
                FullName = vm.FullName,
                UserName = vm.UserName,
            };

            
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            var roleResult = await _userManager.AddToRoleAsync(user, nameof(Roles.User));

            return View();
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm,string returnUrl)
        {
            if (ModelState.IsValid) return View(vm);
            User? user = null;
            if (vm.UsernameOrEmail.Contains('@'))
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            else
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);
            if (result.Succeeded)
            {
                if (result.IsNotAllowed)
                    ModelState.AddModelError("","Username or password is wrong");
                if(result.IsLockedOut)
                    ModelState.AddModelError("","Wait until"+user.LockoutEnd!.Value.ToString("yyyy-MM-dd  HH:mm:ss"));
                return View();
            }
            if(string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return LocalRedirect(returnUrl);
        }
    }
}

