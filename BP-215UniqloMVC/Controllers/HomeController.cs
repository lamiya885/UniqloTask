using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.ViewModels.Slider;
using BP_215UniqloMVC.ViewModels.Common;
using BP_215UniqloMVC.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.ViewModels.Product;

namespace BP_215UniqloMVC.Controllers
{
    public class HomeController(UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new();
            vm.Sliders= await _context.Sliders.Where(x=>!x.IsDeleted).Select(x => new SliderItemVM
            {
                ImageUrl = x.ImageUrl,
                Link=x.Link,
                Title = x.Title,
                Subtitle = x.Subtitle
            }).ToListAsync();

            


           vm.Products = await _context.Products.Where(x => !x.IsDeleted).Select(x => new ProductItemVM
			{
				Discount = x.Discount,
				Id = x.Id,
				ImageUrl = x.CoverImage,
				IsInStock = x.Quantity > 0,
				Name = x.Name,
				Price = x.SellPrice
			}).ToListAsync();
			
            return  View(vm);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }


    }
}
