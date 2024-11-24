using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.Extentions;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace BP_215UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(IWebHostEnvironment _env,UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (vm.CoverFile != null)
            {
                if (vm.CoverFile.IsValidType("image"))
                    ModelState.AddModelError("CoverFile", "File type must be image");
                if (vm.CoverFile.IsValidSize(300))
                    ModelState.AddModelError("CoverFile", "File type must be less than 300");
            }


            if(!ModelState.IsValid) return View();
            Product product = vm;
            product.CoverImage = await vm.CoverFile!.UploadAsync(_env.WebRootPath,"imgs","products");
           
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
