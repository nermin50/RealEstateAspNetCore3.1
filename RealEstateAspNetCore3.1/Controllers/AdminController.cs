using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Controllers
{
    [Authorize(Roles =("admin"))]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
