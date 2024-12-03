using Microsoft.AspNetCore.Identity;

namespace BP_215UniqloMVC.Models
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }

    }
}
