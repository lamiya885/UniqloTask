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
         

            var basketItems=JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["basket"] ?? "[]");
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
            Response.Cookies.Append("basket",JsonSerializer.Serialize(basketItems));
            return Ok();
        

        }

        public async Task<IActionResult> Remove(int Id)
        {
            var basketItems = JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["basket"] ?? "[]");
            var item = basketItems.FirstOrDefault(x => x.Id == Id);
            if (item != null)
            {
               basketItems.Remove(item);
            }
            Response.Cookies.Delete("item");
            return Ok();
        }
    }
}
