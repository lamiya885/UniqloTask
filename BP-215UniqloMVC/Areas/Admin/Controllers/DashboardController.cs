﻿using Microsoft.AspNetCore.Mvc;

namespace BP_215UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}