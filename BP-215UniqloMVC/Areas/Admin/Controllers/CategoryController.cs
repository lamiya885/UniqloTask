﻿using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.Helpers;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BP_215UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Product)]


    public class CategoryController(UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid) return View();
            Category category = new Category();
            category.Name = vm.CategoryName;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? Id)
        {
            if(!Id.HasValue) return BadRequest();
            var data = await _context.Categories.FindAsync(Id);
            if (data is null) return NotFound();
            CategoryUpdateVM vm = new();
            vm.CategoryName = data.Name;
            
            return View(vm);
        }
       
        [HttpPost]
            public async Task<IActionResult> Update(int? Id ,CategoryUpdateVM vm)
        {
            if (!ModelState.IsValid) return View();
            var entity = await _context.Categories.FindAsync(Id);
            if (entity is null) return NotFound();
            entity.Name = vm.CategoryName;

            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (!Id.HasValue) return NotFound();
                    
            if(await _context.Products.AnyAsync(x => x.Id == Id))
            {
                _context.Categories.Remove(new Models.Category { Id = Id.Value });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPatch]
        public async Task<IActionResult> Show(int? Id)
        {
            if (!Id.HasValue) return BadRequest();

            var data = await _context.Categories.FindAsync(Id);

            if (data is null) return View();
            data.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Hide(int? Id)
        {
            if (!Id.HasValue) return BadRequest();

            var data = await _context.Categories.FindAsync(Id);

            if (data is null) return View();
            data.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

    }
}
