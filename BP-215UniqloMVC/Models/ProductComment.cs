using System.ComponentModel.DataAnnotations;

namespace BP_215UniqloMVC.Models
{
    public class ProductComment
    {
        public int Id { get; set; }
        [Range(0,500)]
        public string Comment {  get; set; }
        public int? ProductId {  get; set; }
       public string? UserId {  get; set; }
        public User? User { get; set; }
    }
}
