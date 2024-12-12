using System.Net;
using System.Net.Mail;
using System.Text;
using BP_215UniqloMVC.Enums;
using BP_215UniqloMVC.Helpers;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.Services.Abstract;
using BP_215UniqloMVC.ViewModels.Auths;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BP_215UniqloMVC.Controllers
{
    public class AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager,IOptions<SmtpOptions> opts,IEmailService _service) : Controller
    {
        readonly SmtpOptions _smtpOpt= opts.Value;
        bool isAuthenticated => User.Identity?.IsAuthenticated ?? false;
        public IActionResult Register()
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View();

            User user = new User
            {
                Email = vm.Email,
                FullName = vm.FullName,
                UserName = vm.UserName,
                ProfileImageUrl = "photo.jpg",
            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            var roleResult = await _userManager.AddToRoleAsync(user, nameof(Roles.User));

            if (roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await   _service.SendEmailConfirmation(user.Email,user.UserName, token);
                return Content("Email sent");

          
        }
        public async Task<IActionResult> Login()
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl = null)
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid) return View();

            User? user = null;

            if (vm.UsernameOrEmail.Contains('@'))
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            else
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);

            if (user is null)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }


            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsNotAllowed)
                    ModelState.AddModelError("", "You must confirm your account");
                if (result.IsLockedOut)
                    ModelState.AddModelError("", "Wait until" + user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));

                return View();
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", new { Controller = "Dashboard", Area = "Admin" });
                }
                return RedirectToAction("Index", "Home");
            }
          string token= await   _userManager.GenerateEmailConfirmationTokenAsync(user);
            return LocalRedirect(returnUrl);
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Test()
        {
            //SmtpClient smtp = new();
            //smtp.Host = _smtpOpt.Host;
            //smtp.Port = _smtpOpt.Port;
            //smtp.EnableSsl = true;
            //smtp.Credentials = new NetworkCredential(_smtpOpt.Sender, _smtpOpt.Password);
            //MailAddress from = new MailAddress(_smtpOpt.Sender, "Yaya support");
            //MailAddress to = new("lamiyahasanza@gmail.com");
            //MailMessage msg = new MailMessage(from, to);
            //msg.Subject = "Security alert!";
            //msg.Body = "<p>Change your password immediatly! From this <a>link</a></p>";
            //msg.IsBodyHtml = true;

            //smtp.Send(msg);
            //return Ok("Alindi");

            return Ok();

        }
        public async Task<IActionResult> VerifyEmail(string token ,string user)
        {
            var entity = await _userManager.FindByNameAsync(user);
            if (entity is null) return BadRequest();
           var result= await _userManager.ConfirmEmailAsync(entity, token);
            if(!result.Succeeded)
            {
                StringBuilder sb= new StringBuilder();
                foreach(var item in result.Errors )
                {
                    sb.AppendLine(item.ToString());
                }
                return Content(sb.ToString());
            }
            await _signInManager.SignInAsync(entity, true);
            return RedirectToAction("Index","Home");
        }

    }
}

