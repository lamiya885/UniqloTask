using System.ComponentModel.DataAnnotations;

namespace BP_215UniqloMVC.ViewModels.Auths
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
