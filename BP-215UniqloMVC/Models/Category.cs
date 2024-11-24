using System.ComponentModel.DataAnnotations;

namespace BP_215UniqloMVC.Models
{
    public class Category:BaseEntity
    {
        [MaxLength(32,ErrorMessage ="Name must be less than 32")]
        public string Name { get; set; } = null!;
        public IEnumerable<Product>? Products { get; set; }

    }
}
