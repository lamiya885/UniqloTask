namespace BP_215UniqloMVC.Services.Abstract
{
    public interface IEmailService
    {
        Task SendAsync(string reciever, string body);
       
    }
}
