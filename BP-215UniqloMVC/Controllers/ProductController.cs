using System.Security.Claims;
using BP_215UniqloMVC.DataAccess;
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
            if (data is null) return NotFound();
            /*DetailsVM vm = new();
            vm.Discount = data.Discount;
            vm.Price=data.SellPrice;
            vm.Images = data.Images;
            vm.ProductName=data.Name;*/
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
        //public async Task<IActionResult> Comment(int productId, string Comment)
        //{
        //    string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //    var data = await _context.ProductComment.Where(x => x.UserId == userId && x.ProductId == productId).FirstOrDefaultAsync();
        //    if (data is null)
        //    {
        //        await _context.ProductComment.AddAsync(new Models.ProductComment
        //        {
        //            UserId = userId,
        //            Comment = Comment,
        //            ProductId = productId
        //        });
        //    }
        //    else
        //    {
        //        data.comment = Comment;
        //    }
        //    return View();
        //}
    }
}
