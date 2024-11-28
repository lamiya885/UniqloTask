using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.Extentions;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BP_215UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(IWebHostEnvironment _env,UniqloDbContext _context) : Controller
    {
		public async Task<IActionResult> Index()
        {
            await _context.Products.Include(x=>x.Category).ToListAsync();
            return View( await _context.Products.ToListAsync());
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
                if (!vm.CoverFile.IsValidType("image"))
                    ModelState.AddModelError("CoverFile", "File type must be image");
                if (!vm.CoverFile.IsValidSize(300))
                    ModelState.AddModelError("CoverFile", "File type must be less than 300");
            }
            if(!ModelState.IsValid) return View();

            Product product = vm;
            product.CoverImage = await vm.CoverFile!.UploadAsync(_env.WebRootPath,"imgs","products");
           
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update (int Id,ProductUpdateVM vm)
        {
            if(vm.CoverFile!=null)
            {
                if (!vm.CoverFile.IsValidType("image"))
                    ModelState.AddModelError("CoverFile", "File type must be image ");
                if (!vm.CoverFile.IsValidSize(300))
                    ModelState.AddModelError("CoverFile","File type must be less than 300");

            }
            if (!ModelState.IsValid) return View();

             var entity = await _context.Products.FindAsync(Id);
            
            if (entity is null) return NotFound();
            entity.CoverImage = await vm.CoverFile!.UploadAsync(_env.WebRootPath,"imgs","products");
            entity.Name= vm.Name;
            entity.Description= vm.Description;
            entity.CostPrice = vm.CostPrice;
            entity.SellPrice=vm.SellPrice;
            entity.Quantity = vm.Quantity;
            entity.Discount = vm.Discount;
            entity.CategoryId = vm.CategoryId;


            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete (int Id)
        {
            if(await _context.Products.AnyAsync(x=>x.Id==Id))
            {
                _context.Products.Remove(new Product { Id = Id });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
      
        public async Task<IActionResult> Hide (int? Id)
        {
            if (!Id.HasValue) return BadRequest();
            var data = await _context.Products.FindAsync(Id);
            if (data is null) return View();

            data.IsDeleted= true;
             await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        
        public async Task<IActionResult> Show(int? Id )
        {
            if (!Id.HasValue) return BadRequest();

            var data = await _context.Products.FindAsync(Id);

            if (data is null) return View();


            data.IsDeleted = false;



            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));

        }
       
    }
}
