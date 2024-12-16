using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.ViewModels.Basket;
using BP_215UniqloMVC.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ProductItemVM = BP_215UniqloMVC.ViewModels.Basket.ProductItemVM;

namespace BP_215UniqloMVC.ViewComponnent
{
    public class HeaderViewComponent(UniqloDbContext _context):ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basketIds = JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["basket"] ?? "[]");

         var prods=   await _context.Products.Where(x => basketIds.Select(y => y.Id).Any(y => y == x.Id)).Select(x => new ProductItemVM
            {
                Id=x.Id,
                Name=x.Name,
                Discount=x.Discount,
                ImageUrl=x.CoverImage,
                SellPrice=x.SellPrice,

            }).ToListAsync();
            foreach (var item in prods)
            {
              item.Count= basketIds!.FirstOrDefault(x => x.Id == item.Id)!.Count;
            }

            return View(prods);
        }
    }
}
