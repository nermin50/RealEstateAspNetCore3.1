using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public PartialViewResult PartialFilter()
        {
            ViewBag.citylist = new SelectList(CityGet(), "CityId", "Name");
            ViewBag.statuslist = new SelectList(statusGet(), "StatusId", "StatusName");
            return PartialView();

        }
        public List<City> CityGet()
        {
            List<City> cities = _context.cities.ToList();
            return cities;
        }
        public ActionResult DistrictGet(int CityId)
        {
            List<District> districtlist = _context.districts.Where(x => x.CityId == CityId).ToList();
            ViewBag.districtListesi = new SelectList(districtlist, "DistrictId", "DistrictName");

            return PartialView("DistrictPartial");
        }

        public ActionResult NgbhdGet(int districtid)
        {
            List<Neighborhood> neighborhoodlist = _context.neighborhoods.Where(x => x.DistrictId == districtid).ToList();
            ViewBag.nghbdlistesi = new SelectList(neighborhoodlist, "NeighborhoodId", "NeighborhoodName");

            return PartialView("NgbhdPartial");
        }

        public List<Status> statusGet()
        {
            List<Status> statuses = _context.Status.ToList();
            return statuses;
        }
        public ActionResult typeGet(int statusid)
        {
            // because status sub table of Type 
            List<Tip> typelist = _context.Tips.Where(x => x.StatusId == statusid).ToList();
            ViewBag.typelistesi = new SelectList(typelist, "TypeId", "TypeName");


            return PartialView("TypePartial");
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
        /******************************************************************************/
        // Emlakın durumlarını( Kiralik)  Navbar listeleme fonksyonudur 
        public PartialViewResult StatusName1()
        {
            var statusname1 = _context.Tips.Where(i => i.StatusId == 1).Include(a => a.Status).FirstOrDefault();
            return PartialView(statusname1);
        }
        // Emlakın durumlarını( Satlık ) Navbar listeleme fonksyonudur 
        public PartialViewResult StatusName2()
        {
            var statusname2 = _context.Tips.Where(i => i.StatusId == 2).Include(a => a.Status).FirstOrDefault();
            return PartialView(statusname2);
        }
        /**************************************************************************************/
    }
}
