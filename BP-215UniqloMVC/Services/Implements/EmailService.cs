using BP_215UniqloMVC.Helpers;
using BP_215UniqloMVC.Services.Abstract;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BP_215UniqloMVC.Services.Implements
{
    public class EmailService : IEmailService
    {
        readonly SmtpClient smtp;
        readonly MailAddress _from;
        public EmailService(IOptions<SmtpOptions> option)
        {
           var opt=option.Value;
            smtp = new ();
            smtp.Host=opt.Host;
            smtp.Port=opt.Port;
            smtp.Credentials = new NetworkCredential(opt.Sender, opt.Password);
            smtp.EnableSsl=true;
            _from = new MailAddress(opt.Sender,"Uniqlo");
        }

        public async Task SendAsync(string reciever,string body)
        {
           
            MailAddress from = new MailAddress("lamiyabh-bp215@code.edu.az", "Yaya support");
            MailAddress to = new("lamiyahasanza@gmail.com");
            MailMessage msg = new MailMessage(from, to);
            msg.Subject = "Security alert!";
            msg.Body = "<p>Change your password immediatly! From this <a>link</a></p>";
            msg.IsBodyHtml = true;
            smtp.Send(msg);
            return Task.CompletedTask ;
        }

        public Task SendEmailConfirmationAsync (string reciever)
        {

        }
         
    }
}
