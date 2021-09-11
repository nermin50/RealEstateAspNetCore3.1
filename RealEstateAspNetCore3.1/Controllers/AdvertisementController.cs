using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstateAspNetCore3._1.Models;

namespace RealEstateAspNetCore3._1.Controllers
{
    // Yetkilendirme işlemi bu kontrollere admin ve normal kullanıcı kullanabilir 
    [Authorize(Roles = ("admin,user"))]
    public class AdvertisementController : Controller
    {
        // Veritabanına Connection değişkeni 
        private readonly DataContext _context;
        // dosya yükleme için kullanılır 
        private IHostingEnvironment Environment;

        // Controllerin Konstraktoru
        public AdvertisementController(DataContext context , IHostingEnvironment _environment)
        {
            Environment = _environment;

            _context = context;
        }
        /*************ilana ait resimler listeleme ve ekleme işlemi ***********************/
        public ActionResult Images(int id)
        {
            // Gelen idnin ilanına ait resimleri getir 
            var adv = _context.advertisements.Where(i => i.AdvId == id).ToList();
            var rsml = _context.advPhotos.Where(i => i.AdvId == id).ToList();

            // resimleri Listele
            ViewBag.rsml = rsml;
            // ilanları listele
            ViewBag.adv = adv;
            // Sayfaya Git
            return View();


        }
        // ilana Yeni Resim ekleme Fonskyonu
        // Resimleri toplu yada tek olarak eklenir
        [HttpPost]
        public ActionResult Images(int id, List<IFormFile> file)
        {
            //Eğer yüklenen dosya boş değil ise
            if (file != null)
            {
                // wwwroot yolunu bul
                string wwwPath = this.Environment.WebRootPath;
                // Bu değişken üzerine kaydetme işlemi yapılacak
                string contentPath = this.Environment.ContentRootPath;
              // resimleri Uploads klasörüne kaydedecek
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                // eğer Uploads klasörü yoksa
                if (!Directory.Exists(path))
                {
                    //Yeni bir Uploads klasörü oluştur
                    Directory.CreateDirectory(path);
                }
                // Toplu olarak resimleri kaydetmek için foreach döngüsünü kullandım
                List<string> uploadedFiles = new List<string>();
                foreach (IFormFile postedFile in file)
                {
                    // Resimin adını getir ve fileName Değişkene Kaydet
                    string fileName = Path.GetFileName(postedFile.FileName);
                    // İlana ait resim nesnesini oluştur
                    AdvPhoto rsm = new AdvPhoto();
                    // Tek tek resimlerini adını al ve kaydet
                    rsm.AdvPhotoName = fileName.ToString();

                    // ilişkilendirme 
                    rsm.AdvId = id;
                    // Ekle işlemni yap
                    _context.advPhotos.Add(rsm);
                    //işlemleri kaydet
                    _context.SaveChanges();
                    // resimleri uploads klasörüne kaydet yukarıda sadece resimlerin
                    //adını kaydeder burda ise resimi komple klasöre ekler
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                       
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    }
                }

                // Images Sayfasına git
                return RedirectToAction("Images");
            }

            ViewBag.alert = "Please insert Photo.....";
            return RedirectToAction("Index");

        }
        /*************ilana ait resimler listeleme ve ekleme işlemi Bitti ***********************/

        // ilana ait resimleri sileme işlemi 
        public ActionResult Deleteimages(int? id , int? advId)
        {
            // Eğer gelen resim idisi null ise
            if (id == null)
            {
                // Bad Request 
                return BadRequest("request is incorrect");
            }
            // ilana ait resimleri bul ve getir 
            AdvPhoto advphoto = _context.advPhotos.Find(id);
            // eğer resim bulunmadı ise
            if (advphoto == null)
            {
                //Hata gösteirir
                return NotFound();
            }

            // Resimi sil
            _context.advPhotos.Remove(advphoto);
            //işlemi kaydet
            _context.SaveChanges();
            // Images Sayfasına git
            return RedirectToAction("Images" , new { id = advId });
        }

        // Aşağıdaki metodlar City District Nghb Status Tip GET
        // Advanced Filtre ve Create sayfasında kullanıldı
        // Bütün Şehirleri listeleme fonksyonu
        public List<City> CityGet()
        {
            //Şehirleri listele
            List<City> cities = _context.cities.ToList();
            return cities;
        }
        // Semt listeleme işlemi 
        public ActionResult DistrictGet(int CityId)
        {
            // şehirlere ait işlemeri listele
            List<District> districtlist = _context.districts.Where(x => x.CityId == CityId).ToList();
            ViewBag.districtListesi = new SelectList(districtlist, "DistrictId", "DistrictName");
            // Semtleri Partial View Sayfasına gönder
            return PartialView("DistrictPartial");
        }

        // Mahallar listeleme işlemi
        public ActionResult NgbhdGet(int districtid)
        {
            List<Neighborhood> neighborhoodlist = _context.neighborhoods.Where(x => x.DistrictId == districtid).ToList();
            ViewBag.nghbdlistesi = new SelectList(neighborhoodlist, "NeighborhoodId", "NeighborhoodName");

            return PartialView("NgbhdPartial");
        }

        //  Durum  Listeleme işlemi
        public List<Status> statusGet()
        {
            // Bütün durumlaru getir ve listele 
            List<Status> statuses = _context.Status.ToList();
            return statuses;
        }
        
        //  Tipler Listeleme işlemi
        public ActionResult typeGet(int statusid)
        {
            //Duruma ait Tiplari listele
            // because status sub table of Type 
            List<Tip> typelist = _context.Tips.Where(x => x.StatusId == statusid).ToList();
            ViewBag.typelistesi = new SelectList(typelist, "TypeId", "TypeName");

            // Tip partial view sayfasına git 
            return PartialView("TypePartial");
        }



        /************************ilanları  Listeleme metodu  **************************************************/

        // GET: Advertisement
        public async Task<IActionResult> Index()
        {
            // kullanıcı adını getir
            var username = User.Identity.Name;
            // kullanıcıya ait ilanları listele 
            var dataContext = _context.advertisements.Where(i => i.UserName == username).Include(a => a.Neighborhood).Include(b => b.Neighborhood.District)
                .Include(c => c.Neighborhood.District.City).Include(n=> n.Tip).Include(m => m.Tip.Status);
            // ilanların listesini gönder sayfaya 
            return View(await dataContext.ToListAsync());
        }
        /************************ilanları  Listeleme metodu  Bitti  **************************************************/


        /************************ilan detaylar işlemi **************************************************/

        // GET: Advertisement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // eğer gelen id null ise 
            if (id == null)
            {
                //veri bulunmadı 
                return NotFound();
            }
            // id ye ait ilan verilerini getir 
            var advertisement = await _context.advertisements
                .Include(a => a.Neighborhood)
                .FirstOrDefaultAsync(m => m.AdvId == id);
            // ilan verileri yoksa
            if (advertisement == null)
            {
                // ilan bulunmadı
                return NotFound();
            }
            // ilanı gönder sayfaya
            return View(advertisement);
        }
        /************************ilan Detaylar listeleme işelemi bitti **************************************************/


        /************************Create işlemi **************************************************/

        // GET: Advertisement/Create
        public IActionResult Create()
        {
            //Get Metodu Create Sayfasını Gösterir 

            //Burda veritabanındaki bulunan Şehirlerin  verilerini Listeler 
            ViewBag.citylist = new SelectList(CityGet(), "CityId", "Name");
            // Giriş yapan kullanıcı Adını Listeler 
            ViewBag.Name = User.Identity.Name;
            // Emlak Durumlarını Listeler ( Status = Durum ) 
            ViewBag.statuslist = new SelectList(statusGet(), "StatusId", "StatusName"); return View();
        }

        // POST: Advertisement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdvId,Description,Price,NumOfRoom,NumOfBath,Credit,Area,Floor,Feature,Telephone,Addres,CityId,DistrictId,StatusId,NeighborhoodId,TypeId")] Advertisement advertisement)
        {
            // Post Metodunda Post işlemi yapılyor 
            // eğer Modelin Durumu Doğru ise yani bir hata yoksa aşağıdaki işlemleri yap 
            if (ModelState.IsValid)
            {
                // Kullanıcı adını Getir 
                advertisement.UserName = User.Identity.Name;
                // ilan modelini komple veri tabanına ekle 
                _context.Add(advertisement);
                //işlemleri kaydet
                await _context.SaveChangesAsync();
                // Index Sayfsına git 
                return RedirectToAction(nameof(Index));
            }
            // Mahalları listeleme işlemi  
            ViewData["NeighborhoodId"] = new SelectList(_context.neighborhoods, "NeighborhoodId", "NeighborhoodId", advertisement.NeighborhoodId);
          // ilan modeli gönder 
            return View(advertisement);
        }
        /************************Create işlemi bitti **************************************************/


        /************************Güncelleme **************************************************/
        // (GET): Advertisement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Eğer gelen id Null ise 
            if (id == null)
            {
                // Veri Not Found yani yok 
                return NotFound();
            }
            // Find : Veritabanına bağla ve Bu id'yi Bul ve Getir 
            var advertisement = await _context.advertisements.FindAsync(id);
            // Eğer gelen veri Null ise  
            if (advertisement == null)
            {
                // Veri bulunmadı 
                return NotFound();
            }
            // Mahalları Listele 
            ViewData["NeighborhoodId"] = new SelectList(_context.neighborhoods, "NeighborhoodId", "NeighborhoodId", advertisement.NeighborhoodId);
           // ilan modeli gönder sayafaya 
            return View(advertisement);
        }

        // POST: Advertisement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdvId,Description,Price,NumOfRoom,NumOfBath,Credit,Area,Floor,Feature,Telephone,Addres,CityId,DistrictId,StatusId,NeighborhoodId,TypeId")] Advertisement advertisement)
        {
            // Post Metodu 
            // eğer post yaptığında id yi bulmazsa 
            if (id != advertisement.AdvId)
            {
                // Günceleme işlemi yapılamdı 
                return NotFound();
            }

            //Eğer güncelleme işlemi doğru ise
            if (ModelState.IsValid)
            {
                try
                {
                    // ilan verileri güncelle 
                    _context.Update(advertisement);
                    // işlemleri kaydet
                    await _context.SaveChangesAsync();
                }
                // Eğer güncelleme işlemini yaparakn bir hata yakalarsa hata mesajı gösterir 
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertisementExists(advertisement.AdvId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Index Sayafsına yönlendir
                return RedirectToAction(nameof(Index));
            }
            // Mahalları listele
            ViewData["NeighborhoodId"] = new SelectList(_context.neighborhoods, "NeighborhoodId", "NeighborhoodId", advertisement.NeighborhoodId);
           // ilan modelini gönder
            return View(advertisement);
        }
        /************************Güncelleme işlemi bitti  **************************************************/


        /************************Silme işlemi **************************************************/

        // GET: Advertisement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // eğer gelen id null ise 
            if (id == null)
            {
                // veri bulunmadı 
                return NotFound();
            }
            // Veriyi getir 
            var advertisement = await _context.advertisements
                // ilan tablosuna ait tabloları getir
                .Include(a => a.Neighborhood)
                .FirstOrDefaultAsync(m => m.AdvId == id);
            //eğer ilan null ise 
            if (advertisement == null)
            {
                // ilan bulunmadı
                return NotFound();
            }
            // ilan modelini gönder
            return View(advertisement);
        }

        // POST: Advertisement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Konfirmasyon silme işlemini yap 
            // id ye ait veriyi getir 
            var advertisement = await _context.advertisements.FindAsync(id);
            // veriyi sil
            _context.advertisements.Remove(advertisement);
            //işlemi kaydet
            await _context.SaveChangesAsync();
            // index sayfasına git
            return RedirectToAction(nameof(Index));
        }
        /************************Silme işlemi Bitti  **************************************************/

        // Veritabanın iletişim kapatma metodu 
        private bool AdvertisementExists(int id)
        {
            return _context.advertisements.Any(e => e.AdvId == id);
        }
    }
}
