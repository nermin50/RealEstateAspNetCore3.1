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
        // Database bağalntısı 
        private readonly DataContext _context;


        // Home Controllerin Konstraktörü
        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        //Navbardaki bulunan Menü filtrelem 
        public ActionResult MenuFilter(int id)
        {
            //önce bütün resimleri veritabanından listeler
            var imgs = _context.advPhotos.ToList();
            // bu resimleri ViewBag 'e ekler
            ViewBag.imgs = imgs;
            //ondan sonra filtreleme işlemi yapar ve sonucları filter değişkene yükler
            var filter = _context.advertisements.Where(i => i.TypeId == id).Include(l => l.Neighborhood).Include(n => n.Neighborhood.District).
                Include(m => m.Neighborhood.District.City).Include(e => e.Tip).Include(e => e.Tip.Status).ToList();
            //Filter değişkeni sayfaya yönlendirir
            return View(filter);
        }
        //Gelişmiş Filterleme fonksyonu 
        public ActionResult Filter(int min, int max, int cityid, int districtid, int NeighborhoodId, int StatusId, int typeid)
        {
             // önce resimleri list olarak bir değşkene atıyorum
            var imgs = _context.advPhotos.ToList();
            ViewBag.imgs = imgs;
            // Arayüzden gelen verileri alıp  filtrelme işlemi yapar ve sonucu filter değişkene atılyor 
            // aramayı ilan tablosu ile bağlanan bütün tabloları dahil etmişimdir 
            var filter = _context.advertisements.Where(x => x.Price >= min && x.Price <= max
            && x.CityId == cityid
            && x.DistrictId == districtid
            && x.NeighborhoodId == NeighborhoodId
            && x.StatusId == StatusId
            && x.TypeId == typeid).Include(l => l.Neighborhood).Include(n => n.Neighborhood.District).
                Include(m => m.Neighborhood.District.City).Include(e => e.Tip).Include(e => e.Tip.Status).ToList();

            //sonucu sayfaya yönlendiryorum
            return View(filter);

        }
        // Bu fonksyon şehir tablosundaki bulunan tüm şehirleri listeler
        public List<City> CityGet()
        {
            List<City> cities = _context.cities.ToList();
            return cities;
        }
        // Bu fonksyonda gelen şehir idisni alıp tüm semtleri listeler 
        //ve Partial sayfasına gönderir
        public ActionResult DistrictGet(int CityId)
        {
            List<District> districtlist = _context.districts.Where(x => x.CityId == CityId).ToList();
            ViewBag.districtListesi = new SelectList(districtlist, "DistrictId", "DistrictName");

            return PartialView("DistrictPartial");
        }
        // Bu fonksyonda gelen semtin idisini alınır ve semte ait olan tüm mahalları listeler
        //ve partila sayfasına gönderir
        public ActionResult NgbhdGet(int districtid)
        {
            List<Neighborhood> neighborhoodlist = _context.neighborhoods.Where(x => x.DistrictId == districtid).ToList();
            ViewBag.nghbdlistesi = new SelectList(neighborhoodlist, "NeighborhoodId", "NeighborhoodName");

            return PartialView("NgbhdPartial");
        }

        // Bu fonksyonda Durum tablosunda bulunan bütün durumları listeler 
        public List<Status> statusGet()
        {
            List<Status> statuses = _context.Status.ToList();
            return statuses;
        }
        // bu fonksyonda gelen durum idisini alıp bütün tipleri listeler 
        public ActionResult typeGet(int statusid)
        {
            // because status sub table of Type 
            List<Tip> typelist = _context.Tips.Where(x => x.StatusId == statusid).ToList();
            ViewBag.typelistesi = new SelectList(typelist, "TypeId", "TypeName");


            return PartialView("TypePartial");
        }


        // Arama Motorun Fonksyonudur 
        public ActionResult Search(string s)
        {
           // önce bütün resimlerin adılarını ViewBag'a yükliyorum 
            var imgs = _context.advPhotos.ToList();
            ViewBag.imgs = imgs;
            // Kullanıcı tarafından yazılan keilmeyi alıp aşağıdaki arama işlemi yapılmaktadır 
            var search = _context.advertisements.Include(m => m.Neighborhood).Include(b => b.Neighborhood.District)
                .Include(c => c.Neighborhood.District.City).Include(n => n.Tip).Include(m => m.Tip.Status).AsQueryable(); ;
            //Eğer gelen kelime boş değilse arama işlemi yap Kodudur 
            if (!string.IsNullOrEmpty(s))
            {
                search = (search.Where(i => i.Description.Contains(s) || i.Neighborhood.NeighborhoodName.Contains(s)
                || i.Neighborhood.District.City.Name.Contains(s)));
            }

            return View(search.ToList());
        }

        // anasayfayı gösteren fonksyonudur 
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
            // ilanın butonuna tıkladıktan sonra gelen idyi alıp id'ya ait olan ilanın bilgilerini bulup ve adv değişkenine yüklenyor 
            var adv = _context.advertisements.Where(i => i.AdvId == id).Include(l => l.Neighborhood).Include(n => n.Neighborhood.District).
                Include(m => m.Neighborhood.District.City).Include(e => e.Tip).Include(e => e.Tip.Status).FirstOrDefault();
            // Burada ilana ait olan bütün resimleri alıp Silder'e yükleniyor 
            var imgs = _context.advPhotos.Where(i => i.AdvId == id).ToList();
            ViewBag.imgs = imgs;
            //Sonucu Sayfaya yönlendiryorum 
            return View(adv);

        }

        // iletişim sayfasını  gösterme fonksyoudur // HTTPGET fonksyonu  
        public ActionResult ContactUs()
        {
            return View();
        }
        //Burada Kullanıcı tarafından yazılan mesajı ve bilgilerini Mailimze gönderen fonksyonudur 
        [HttpPost]

        public ActionResult Contact(ContactUs contact)
        {
            // Burada MailMessage nesnesi üzerinden Mail'i oluşturyoruz 
            var mail = new MailMessage();
            // Mail gönderme işlemi yapılyor 
            // gönderici bizim Mailmiz - Alıcı : aynı mail 
            // Bu işlem kullancı tarafından yaılan epost gönderilmesini kolaylaştırmak için yazılmıştır 
            var loginInfo = new NetworkCredential("ddeneme96390@gmail.com", "deneme123");
            // Gönderici yazığımız email ve şifresi 
            mail.From = new MailAddress(contact.Email);
            // Alıcı 
            mail.To.Add(new MailAddress("ddeneme96390@gmail.com"));
            // emai'in başlığı 
            mail.Subject = "Mail From Website ";
            // Emai'in templati burada basit bir templat hazırladım 
            mail.IsBodyHtml = true;
            string body = "Sender Name : " + contact.Name + "<br>" +
                            "Email :  " + contact.Email + "<br>" +
                            "Telephone : " + contact.Telephone + "<br>" +
                            "Messege :: <b>" + contact.Message + "</b>";
            mail.Body = body;
            // emaili göndermek için   google'e ait portları kullandım 
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail);
            // email gönderildi ise başarılı mesaji gösterecektir 
            TempData["SuccessfulSende"] = "işlem başarılı ";
            // Mail'i gönderdikten sonra anasayfya yönlendirecektir bizi 
            return RedirectToAction("Index");
        }

    }
}
