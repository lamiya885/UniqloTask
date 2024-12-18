using Microsoft.AspNetCore.Mvc;

namespace BP_215UniqloMVC.Services.Abstract
{
    public interface IEmailService
    {
        //Task SendAsync(string reciever, string body);
       public Task<IActionResult> SendEmailConfirmation(string reciver,string name,string token);
    }
}
