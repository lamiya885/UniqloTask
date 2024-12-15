using BP_215UniqloMVC.Models;

namespace BP_215UniqloMVC.ViewModels.ProductsDetails
{
    public class DetailsVM
    {
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public  string CoverImageUrl { get; set; }

        public ICollection<ProductImage>? Images { get; set; }
        
    }
}
