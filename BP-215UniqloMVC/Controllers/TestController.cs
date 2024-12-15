using Microsoft.AspNetCore.Mvc;

namespace BP_215UniqloMVC.Controllers
{
    public class TestController : Controller
    {
        public IActionResult AddSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
            return Ok();
        }
        public async Task<IActionResult> GetSession(string key)
        {
           return Content(HttpContext.Session.GetString(key));
          
        }
     
    }
}
