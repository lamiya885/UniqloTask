using System.ComponentModel.DataAnnotations;

namespace BP_215UniqloMVC.ViewModels.Auths
{
    public class ResetPasswordVM
    {
            [EmailAddress]
            public string Email { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
             public string ConfirmPassword { get; set; }
            public string Code { get; set; }
      
    }
}
