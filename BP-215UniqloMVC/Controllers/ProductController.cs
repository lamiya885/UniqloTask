using System.Security.Claims;
using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.Models;
using BP_215UniqloMVC.ViewModels.Comment;
using BP_215UniqloMVC.ViewModels.ProductsDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BP_215UniqloMVC.Controllers
{
    public class ProductController(UniqloDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? Id)
        {

            if (!Id.HasValue) return BadRequest();

            var data = await _context.Products
                .Where(x => x.Id == Id.Value && !x.IsDeleted)
                .Include(x => x.Images).Include(x => x.Ratings).Include(x=>x.Comments).FirstOrDefaultAsync();
            if (data is null)
            {
                DetailsVM vm = new()
                {
                    Discount = data.Discount,
                    Price = data.SellPrice,
                    Images = data.Images,
                    ProductName = data.Name,
                    CoverImageUrl=data.CoverImage,

                };
            }
            ViewBag.Rating = 5;
            if (User.Identity?.IsAuthenticated ?? false)
            {
                string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                int rating = await _context.ProductRatings.Where(x => x.UserId == userId && x.ProductId == Id).Select(x => x.Rating).FirstOrDefaultAsync();
                ViewBag.Rating = rating == 0 ? 5 : rating;
            }
            return View(data);
        }
        public async Task<IActionResult> Rating(int productId, int rating)
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var data = await _context.ProductRatings.Where(x => x.UserId == userId && x.ProductId == productId).FirstOrDefaultAsync();
            if (data is null)
            {
                await _context.ProductRatings.AddAsync(new Models.ProductRating
                {
                    UserId = userId,
                    Rating = rating,
                    ProductId = productId
                });
            }
            else
            {
                data.Rating = rating;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { Id = productId });
        }
        public async Task<IActionResult> CommentCreate(int productId, string Comment,CommentCreateVM vm)
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var data = await _context.ProductComments.Where(x => x.UserId == userId && x.ProductId == productId).FirstOrDefaultAsync();

              ProductComment productComment = new ProductComment()
                {
                    FullName = vm.FullName,
                    Email = vm.Email,
                    UserId = userId,
                    Comment = Comment,
                    ProductId = productId
                };
            
            
           await _context.ProductComments.AddAsync(productComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details));
        }
        public async Task<IActionResult> CommentRemove(int? Id)
        {
            if (!Id.HasValue) return BadRequest();
            var data = await _context.ProductComments.FindAsync(Id);
            if (data is null) return NotFound();
             _context.Remove(data);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details));

        }
    }
}
