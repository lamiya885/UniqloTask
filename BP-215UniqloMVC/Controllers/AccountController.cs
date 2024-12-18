using System.Net;
using System.Net.Mail;
using System.Text;
using BP_215UniqloMVC.Enums;
using BP_215UniqloMVC.Helpers;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.Services.Abstract;
using BP_215UniqloMVC.Services.Implements;
using BP_215UniqloMVC.ViewModels.Auths;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BP_215UniqloMVC.Controllers
{
    public class AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager,IOptions<SmtpOptions> opts, IEmailService _service ) : Controller
    {
        readonly SmtpOptions _smtpOpt= opts.Value;
        //readonly IEmailSender emailSender = _emailSender;
       public bool isAuthenticated => User.Identity?.IsAuthenticated ?? false;
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

                return Content("Email sent");

          
        }
        public async Task<IActionResult> Login()
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl )
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

        public IActionResult ForgotPasword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM vm)
        {
            if(vm is null) return NotFound();
            if(!ModelState.IsValid)
            {
                return View();
            }
            
            
           
		var user = await _userManager.FindByEmailAsync(vm.Email);
	
		
           SmtpClient smtp = new SmtpClient();
            smtp.Host=_smtpOpt.Host;
            smtp.Port = _smtpOpt.Port;
            smtp.Credentials = new NetworkCredential(_smtpOpt.Username, _smtpOpt.Password);
            MailAddress from = new MailAddress(_smtpOpt.Username, "Uniqlo");
            MailAddress to = new(vm.Email);
            smtp.EnableSsl = true;


            Random random = new Random();
            string randomCode = random.Next(1000,100000).ToString();

		var code = await _userManager.GeneratePasswordResetTokenAsync(user);
		var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = randomCode }, protocol: HttpContext.Request.Scheme);
            MailMessage message = new MailMessage();
            message.Subject = "Reset Password";
            message.Body = "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>";
            message.IsBodyHtml = true;
           smtp.Send(message);
          
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
          
        }
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        //OZ YAZDIGIN
        //public IActionResult ResetPassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        //{

        //    if (!ModelState.IsValid)  return View(vm);
        //     var user = await _userManager.FindByEmailAsync(vm.Email);
        //    if (user == null)
        //       return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");

        //    var result = await _userManager.ResetPasswordAsync(user, vm.Code, vm.Password);
        //    if (!result.Succeeded)
        //    {
        //        foreach (var err in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, err.Code + " " + err.Description);
        //        }
        //        return View();
        //    }
        //    return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");


        //}

        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid) { return View(); }
            User? user = null;
            var data = await _userManager.FindByEmailAsync(vm.Email);
            user = data;
            if (user == null)
            {
                ModelState.AddModelError("", "Istifadeci tapilmadi");
                return View();
            }
            user.EmailConfirmed = false;
            user.UserName = data!.UserName;
            user.Email = data!.Email;
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, vm.Password);
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _service.SendEmailConfirmation(user.Email, user.UserName, token);


            return RedirectToAction(nameof(CheckGmail));
        }
        public IActionResult CheckGmail()
        {
            return View();
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();

        }
    }

}

