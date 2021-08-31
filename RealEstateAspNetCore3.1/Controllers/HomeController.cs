using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
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


        public ActionResult MenuFilter(int id)
        {
            var imgs = _context.advPhotos.ToList();
            ViewBag.imgs = imgs;
            var filter = _context.advertisements.Where(i => i.TypeId == id).Include(l => l.Neighborhood).Include(n => n.Neighborhood.District).
                Include(m => m.Neighborhood.District.City).Include(e => e.Tip).Include(e => e.Tip.Status).ToList();
            return View(filter);
        }
        public ActionResult Filter(int? min, int? max, int? cityid, int? districtid, int? nghdid, int? stautsid, int? typeid)
        {
            var imgs = _context.advPhotos.ToList();
            ViewBag.imgs = imgs;
            var filter = _context.advertisements.Where(x => x.Price >= min || x.Price <= max
            || x.CityId == cityid
            || x.DistrictId == districtid
            || x.NeighborhoodId == nghdid
            || x.StatusId == stautsid
            || x.TypeId == typeid).Include(l => l.Neighborhood).Include(n => n.Neighborhood.District).
                Include(m => m.Neighborhood.District.City).Include(e => e.Tip).Include(e => e.Tip.Status).ToList();

            return View(filter);

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
        
            return View();
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

       
        //ilanın detayları 
        public ActionResult Details(int id)
        {
            var adv = _context.advertisements.Where(i => i.AdvId == id).Include(l => l.Neighborhood).Include(n => n.Neighborhood.District).
                Include(m => m.Neighborhood.District.City).Include(e => e.Tip).Include(e => e.Tip.Status).FirstOrDefault();
            var imgs = _context.advPhotos.Where(i => i.AdvId == id).ToList();
            ViewBag.imgs = imgs;
            return View(adv);

        }

        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Contact(ContactUs contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("ddeneme96390@gmail.com", "deneme123");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("ddeneme96390@gmail.com"));
            mail.Subject = "Mail From Website ";
            mail.IsBodyHtml = true;
            string body = "Sender Name : " + contact.Name + "<br>" +
                            "Email :  " + contact.Email + "<br>" +
                            "Telephone : " + contact.Telephone + "<br>" +
                            "Messege :: <b>" + contact.Message + "</b>";
            mail.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail);
            TempData["SuccessfulSende"] = "işlem başarılı ";
            return RedirectToAction("Index");
        }

    }
}
