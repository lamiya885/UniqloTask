using Azure.Core;
using BP_215UniqloMVC.Helpers;
using BP_215UniqloMVC.Services.Abstract;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BP_215UniqloMVC.Services.Implements
{
    public class EmailService : IEmailService
    {
        readonly SmtpClient smtp;
        readonly MailAddress _from;
        readonly HttpContext Context;
        public EmailService(IOptions<SmtpOptions> option,IHttpContextAccessor acc)
        {
           var opt=option.Value;
            smtp = new (opt.Host,opt.Port);
            smtp.Credentials = new NetworkCredential(opt.Sender, opt.Password);
            smtp.EnableSsl=true;
            _from = new MailAddress(opt.Sender,"Uniqlo");
            Context = acc.HttpContext;
        
        }

        public  Task SendAsync(string reciever,string body)
        {
           
            MailAddress from = new MailAddress("lamiyabh-bp215@code.edu.az", "Yaya support");
            MailAddress to = new(reciever);
            MailMessage msg = new MailMessage(from, to);
            msg.Subject = "Security alert!";
            msg.Body = "<p>Change your password immediatly! From this <a>link</a></p>";
            msg.IsBodyHtml = true;
            smtp.Send(msg);

            string url = Context.Request.Scheme + "://" + Context.Request.Host + "/Account/Verifyzemail?token";
            return Task.CompletedTask ;
        }

       

        public  Task  SendEmailConfirmation(string reciever,string name,string token)
        {
            MailAddress to = new(reciever);
            MailMessage msg = new MailMessage(_from,to);
            msg.IsBodyHtml = true;
            msg.Subject = "Confirm your email adress";
            string url= Context.Request.Scheme + "://" + Context.Request.Host + "/Account/VerifyEmail?token=" + token+"&user";
           EmailTemplates.VerifyEmail.Replace("__$name", name).Replace("__$link",url);
           msg.Body = EmailTemplates.VerifyEmail;
            //Context.Request.Url = Context.Request.Url.Scheme + "://" + Context.Request.Url.Authority + Context.Request.ApplicationPath.TrimEnd('/') + "/";
            smtp.Send(msg);
           return Task.CompletedTask;
        }

    }
}


