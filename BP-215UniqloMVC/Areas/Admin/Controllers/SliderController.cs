using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.Extentions;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.ViewModels.Slider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BP_215UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            return View(await  _context.Sliders.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create( SliderCreateVM vm)
        {
            if(!vm.File.ContentType.StartsWith("image"))
              ModelState.AddModelError("File","File type must be image");
            if (vm.File.Length > 600 * 1024)
              ModelState.AddModelError("File", "File type must be less than 600kb");
            if(!ModelState.IsValid) return View();
          
           string newFileName= Path.GetRandomFileName()+ Path.GetExtension(vm.File.FileName);

            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs","slider",newFileName)))
            {
                await vm.File.CopyToAsync(stream);

            }
            Slider slider = new Slider()
            {
                ImageUrl = newFileName,
                Link = vm.Link,
                Title = vm.Title,
                Subtitle = vm.Subtitle
            };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update (int? Id)
        {
            if (Id.HasValue) return BadRequest();
            var data = await _context.Sliders.FindAsync(Id);
            if (data is null) return NotFound();
            SliderUpdateVM vm = new();
            vm.Title = data.Title;
            vm.Subtitle = data.Subtitle;
            vm.Link = data.Link;
           
            
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update( int? id,SliderCreateVM vm)
        {
            if (!id.HasValue) return BadRequest();
            if (!vm.File.IsValidSize(300))
                ModelState.AddModelError("File","size must be less than 300");
            if (!vm.File.IsValidType("image"))
                ModelState.AddModelError("File","Type must be 'image' ");
            if (!ModelState.IsValid) return View(vm);

            var entity = await _context.Sliders.FindAsync(id);
            if(entity is null) return NotFound();
            entity.ImageUrl = await vm.File!.UploadAsync(_env.WebRootPath, "imgs", "sliders");
            entity.Title = vm.Title;
            entity.Subtitle = vm.Subtitle;
            entity.Link = vm.Link;
           
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (!Id.HasValue) return BadRequest();
            if(await _context.Sliders.AnyAsync(x=>x.Id==Id))
            {
                 _context.Sliders.Remove(new Slider { Id = Id.Value });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Hide(int? Id)
        {
            if (!Id.HasValue) return BadRequest();

            var data = await _context.Sliders.FindAsync(Id);

            if (data is null) return View();
            data.IsDeleted=true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Show(int? Id)
        {
            if (!Id.HasValue) return BadRequest();

            var data = await _context.Sliders.FindAsync();

            if (data is null) return View();
            data.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }



    }
}
