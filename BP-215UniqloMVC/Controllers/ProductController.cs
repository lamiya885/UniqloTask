using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.ViewModels.ProductsDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BP_215UniqloMVC.Controllers
{
    public class ProductController (UniqloDbContext _context): Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? Id )
        {
            if(!Id.HasValue)  return BadRequest();

            var data=await _context.Products
                .Where(x=>x.Id== Id.Value && !x.IsDeleted)
                .Include(x=>x.Images).FirstOrDefaultAsync();
            if(data is null ) return NotFound();
            DetailsVM vm = new();
            vm.Discount = data.Discount;
            vm.Price=data.SellPrice;
            vm.Images = data.Images;
            vm.ProductName=data.Name;
            return View(data);
        }
    }
}
