using System.ComponentModel.DataAnnotations;
using BP_215UniqloMVC.Models;

namespace BP_215UniqloMVC.ViewModels.Product
{
    public class ProductCreateVM
    {
        [MaxLength(32,ErrorMessage ="Name must be less than 32"),Required]
        public string Name { get; set; } = null!;
        [MaxLength(50, ErrorMessage = "Description must be less than 50"), Required]
        public string Description { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string? CoverFileUrl { get; set; }
        public IEnumerable<string>? OtherFileUrls { get; set; }

        public IFormFile CoverFile { get; set; } = null!;
        public IEnumerable<IFormFile> OtherFiles { get; set; }

        public int? CategoryId { get; set; }


    }
}
