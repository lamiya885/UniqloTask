using System.Text.Json;
using BP_215UniqloMVC.DataAccess;
using BP_215UniqloMVC.ViewModels.Basket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BP_215UniqloMVC.Controllers
{
    public class BasketController (UniqloDbContext _context): Controller
    {
        public async Task<IActionResult> AddBasket(int Id)
        {
            //  if (!await _context.Products.AnyAsync(x => x.Id == Id)) return NotFound();
            //object obj = new
            //{
            //    Name="Lamiya",
            //    Surname="Hasanzade"

            //};
            //string a = JsonSerializer.Serialize(obj);

            var basketItems=JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["basket"]?? "[]");
           var item=  basketItems.FirstOrDefault(x=>x.Id==Id);
            if (item == null)
            {
                basketItems.Add(new BasketProductItemVM
                {
                    Count = 1,
                    Id = Id
                });

            }
            else
            item.Count++;
            Response.Cookies.Append("basket",Id.ToString());
            return Ok();
            //return Json(new
            //{ 
            //    a,obj
            //});


        }
    }
}
