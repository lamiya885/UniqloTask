using Microsoft.AspNetCore.Mvc;

namespace BP_215UniqloMVC.ViewComponnent
{
    public class HeaderViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
