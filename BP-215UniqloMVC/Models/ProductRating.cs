using System.ComponentModel.DataAnnotations;

namespace BP_215UniqloMVC.Models
{
    public class ProductRating
    {
        public int Id { get; set; }
        [Range(1,5)]
        public int Rating { get; set; }

        public int? ProductId { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
