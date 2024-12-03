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
            ViewBag.Categories= await _context.Categories.Where(x=>!x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
           
            if(vm.OtherFiles!=null && vm.OtherFiles.Any() )
            {
                if(!vm.OtherFiles.All(x=>x.IsValidType("image")))
                {
                   var  fileNames= vm.OtherFiles.Where(x => !x.IsValidType("image")).Select(x => x.FileName);
                    ModelState.AddModelError("OtherFiles", string.Join(",", fileNames) + "are(is) not an image");
               
                }

                if(!vm.OtherFiles.All(x=>x.IsValidSize(500)))
                {
                    var fileName = vm.OtherFiles.Where(x => x.IsValidSize(500)).Select(x => x.FileName);
                    ModelState.AddModelError("OtherFiles", string.Join(",", fileName) + "must be less then 300kb");
                }
            }
            if (vm.CoverFile != null)
            {
                if (!vm.CoverFile.IsValidType("image"))
                    ModelState.AddModelError("CoverFile", "File type must be image");
                if (!vm.CoverFile.IsValidSize(300))
                    ModelState.AddModelError("CoverFile", "File type must be less than 300");
            }
            if (!ModelState.IsValid)
            {
                string NewFileName = Path.GetRandomFileName() + Path.GetExtension(vm.CoverFile.FileName);
                ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
                return View();
            }

            using (Stream stream=System.IO.File.Create(Path.Combine(_env.WebRootPath,"imgs","products")))
            {
                await vm.CoverFile.CopyToAsync(stream);
            }

                Product product = vm;

            product.CoverImage = await vm.CoverFile!.UploadAsync(_env.WebRootPath,"imgs","products");
           
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            List<ProductImage> list = [];
           foreach( var item in vm.OtherFiles)
            {
                string fileName = await item.UploadAsync(_env.WebRootPath, "imgs", "products");
                list.Add(new ProductImage
                {
                    FileUrl = fileName,
                    Product=product
                });
            }
            Product products = new Product
            {
                CategoryId = (int)vm.CategoryId,
                CostPrice = vm.CostPrice,   
                SellPrice = vm.SellPrice,       
                Description = vm.Description,
                Discount = vm.Discount

            };
            await _context.Products.AddRangeAsync([]);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? Id)
        {
            if (!Id.HasValue) return BadRequest();
            var product = await _context.Products.Where(x => x.Id == Id.Value).Select(x => new ProductUpdateVM
            { 
                CategoryId=x.CategoryId,
                Name=x.Name,
                Description=x.Description,
                CostPrice=x.CostPrice,
                SellPrice=x.SellPrice,
                Discount=x.Discount,
                Quantity=x.Quantity,
                OtherFileUrls=x.Images.Select(y=>y.FileUrl)
            }).FirstOrDefaultAsync();

            //var data = await _context.Products.FindAsync(Id);
            //if (data is null) return NotFound();
            //ProductUpdateVM vm = new();
            //vm.Name = data.Name;
            //vm.Description = data.Description;
            //vm.CostPrice = data.CostPrice;
            //vm.SellPrice = data.SellPrice;
            //vm.Discount = data.Discount;
            //vm.Quantity = data.Quantity;
            //vm.CategoryId= data.CategoryId; 

            ViewBag.Categories= await _context.Categories.Where(x=>!x.IsDeleted).ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update (int? Id,ProductUpdateVM vm)
        {
            if(vm.CoverFile!=null)
            {
                if (!vm.CoverFile.IsValidType("image"))
                    ModelState.AddModelError("CoverFile", "File type must be image ");
                if (!vm.CoverFile.IsValidSize(300))
                    ModelState.AddModelError("CoverFile","File type must be less than 300");

            }
            if (!ModelState.IsValid) return NotFound();

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


            ViewBag.Categories= await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();

            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete (int? Id)
        {
            if (Id is null) return BadRequest();
            if(await _context.Products.AnyAsync(x=>x.Id==Id))
            {
                _context.Products.Remove(new Product { Id = Id.Value });
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
       
        public async Task<IActionResult> DeleteImage(int? Id)
        {
            if (!Id.HasValue) return BadRequest();
            var img = await _context.ProductImage.FindAsync(Id.Value);
            if (img == null) return NotFound();
            _context.ProductImage.Remove(img);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
