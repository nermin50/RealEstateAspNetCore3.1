using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Controllers
{
    // yetkilendirem işlemi aşağıdaki fonksyonlar sadece admin tarafından kullanılır
    [Authorize(Roles =("admin"))]
    public class AdminController : Controller
    {
        // Admin panel sayfasını göstermek için kullanılır 
        public IActionResult Index()
        {
            return View();
        }
    }
}
