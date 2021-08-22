using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;


        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var imgs = _context.advPhotos.ToList();
            ViewBag.imgs = imgs;

            var adv = _context.advertisements.Include(m => m.Neighborhood).Include(e => e.Tip).OrderByDescending(i => i.AdvId); ;
            //ModelState.Clear();
            return View(adv.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public PartialViewResult Slider()
        {
            var adv = _context.advertisements.ToList().Take(5);
            var imgs = _context.advPhotos.ToList();
            ViewBag.imgs = imgs;
            return PartialView(adv);
        }
    }
}
