using Azure.Core;
using BP_215UniqloMVC.Helpers;
using BP_215UniqloMVC.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Uniqlo_New.Helpers;

namespace BP_215UniqloMVC.Services.Implements
{
    public class EmailService : IEmailService

    {
        readonly SmtpClient smtp;
        readonly MailAddress _from;
        readonly HttpContext Context;
        public EmailService(IOptions<SmtpOptions> option, IHttpContextAccessor acc)
        {
            var opt = option.Value;
            smtp = new(opt.Host, opt.Port);
            smtp.Credentials = new NetworkCredential(opt.Username, opt.Password);
            smtp.EnableSsl = true;
            _from = new MailAddress(opt.Username, "Uniqlo");
            Context = acc.HttpContext;

        }

        //public async Task<IActionResult> SendAsync(string reciever, string body)
        //{

        //    MailAddress from = new MailAddress("lamiyabh-bp215@code.edu.az", "Yaya support");
        //    MailAddress to = new(reciever);
        //    MailMessage msg = new MailMessage(from, to);
        //    msg.Subject = "Security alert!";
        //    msg.Body = "<p>Change your password immediatly! From this <a>link</a></p>";
        //    msg.IsBodyHtml = true;
        //    smtp.Send(msg);

        //    string url = Context.Request.Scheme + "://" + Context.Request.Host + "/Account/Verifyzemail?token";
        //    return (IActionResult)Task.CompletedTask;
        //}



        public async Task<IActionResult> SendEmailConfirmation(string reciever, string name, string token)
        {
            MailAddress to = new(reciever);
            MailMessage msg = new MailMessage(_from, to);
            msg.IsBodyHtml = true;
            msg.Subject = "Confirm your email adress";
            string url = Context.Request.Scheme + "://" + Context.Request.Host + "/Account/VerifyEmail?token=" + token + "&user="+name;
            msg.Body = EmailTemplates.VerifyEmail.Replace("__$name", name).Replace("__$link", url);

            smtp.Send(msg);
            return (IActionResult)Task.CompletedTask;
        }

    }
}


