﻿using System.Net.Mail;
using BP_215UniqloMVC.Enums;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.ViewModels.Auths;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BP_215UniqloMVC.Controllers
{
    public class AccountController(UserManager<User> _userManager,SignInManager<User> _signInManager) : Controller
    {
        bool isAuthenticated=> User.Identity?.IsAuthenticated ?? false;
        public IActionResult Register()
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
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
            if(isAuthenticated) return RedirectToAction("Index","Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm,string? returnUrl=null)
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
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

            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);
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
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", new {Contoller ="Dahboard",Area="Admin"});
                }

                return RedirectToAction("Index", "Home");
            }

            return LocalRedirect(returnUrl);
        }
        [Authorize]
        public async Task<IActionResult> LogOut ()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Test ()
        {
            SmtpClient smtp = new();
            smtp.Host = "smtp.gmail.com";
            return View();
        }
    }
}

