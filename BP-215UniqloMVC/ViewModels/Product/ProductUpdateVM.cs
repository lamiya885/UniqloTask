﻿namespace BP_215UniqloMVC.ViewModels.Product
{
    public class ProductUpdateVM
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string CoverFileUrl { get; set; }
        public IEnumerable<string>? OtherFileUrls { get; set; }

        public IFormFile CoverFile { get; set; }
        public IEnumerable<IFormFile> OtherFiles { get; set; }


        public int CategoryId { get; set; }
    }
}
