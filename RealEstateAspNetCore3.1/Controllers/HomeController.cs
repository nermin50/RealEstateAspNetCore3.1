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

        public ActionResult Search(string s)
        {
            var imgs = _context.advPhotos.ToList();
            ViewBag.imgs = imgs;
            var search = _context.advertisements.Include(m => m.Neighborhood).Include(b => b.Neighborhood.District)
                .Include(c => c.Neighborhood.District.City).Include(n => n.Tip).Include(m => m.Tip.Status).AsQueryable(); ;
            if (!string.IsNullOrEmpty(s))
            {
                search = (search.Where(i => i.Description.Contains(s) || i.Neighborhood.NeighborhoodName.Contains(s)
                || i.Neighborhood.District.City.Name.Contains(s)));
            }

            return View(search.ToList());
        }
        public IActionResult Index()
        {
            var imgs = _context.advPhotos.ToList();
            ViewBag.imgs = imgs;

            var adv = _context.advertisements.Include(l => l.Neighborhood).Include(n => n.Neighborhood.District).
                Include(m => m.Neighborhood.District.City).Include(e => e.Tip).Include(e => e.Tip.Status).OrderByDescending(i => i.AdvId); ;
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
        //ilanın detayları 
        public ActionResult Details(int id)
        {
            var adv = _context.advertisements.Where(i => i.AdvId == id).Include(l => l.Neighborhood).Include(n => n.Neighborhood.District).
                Include(m => m.Neighborhood.District.City).Include(e => e.Tip).Include(e => e.Tip.Status).FirstOrDefault();
            var imgs = _context.advPhotos.Where(i => i.AdvId == id).ToList();
            ViewBag.imgs = imgs;
            return View(adv);

        }
    }
}
