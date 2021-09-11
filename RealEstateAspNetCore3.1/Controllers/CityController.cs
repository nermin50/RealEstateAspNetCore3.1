using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstateAspNetCore3._1.Models;

namespace RealEstateAspNetCore3._1.Controllers
{
    [Authorize(Roles = ("admin"))] // bütün aşağıdaki işlemleri sadece admin yapabilir  
    public class CityController : Controller
    {
        // veitabanına bağlanamk için aşağıdaki değişkeni kullanyoruz
        private readonly DataContext _context;

        // şehir kontrollerin konstraktörü 
        public CityController(DataContext context)
        {
            _context = context;
        }

        // aşağıdaki fonksyon veritabanındaki bütün şehirleri listemek için kullanılır
        // GET: City
        public async Task<IActionResult> Index()
        {
            //Şehirleri veritabanındaki tablodan alıp değişkene yükler
            return View(await _context.cities.ToListAsync());
        }

        // aşağıdaki  Her şehir detaylarını göstermek için kullanılır
        // GET: City/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // eğer gelen id null ise veri bulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // Şehirin tüm bilgilerini getirir
            var city = await _context.cities
                .FirstOrDefaultAsync(m => m.CityId == id);
            // Eğer şehir null ise veribulunmadı mesajını gösterir
            if (city == null)
            {
                return NotFound();
            }
            // Şehir modelini sayfaya yükler 
            return View(city);
        }

        // Şehir ekleme sayfasını gösetermek için aşağıdaki Get Fonksyonu kullanılır 
        // GET: City/Create
        public IActionResult Create()
        {
            return View();
        }

        // Şehir ekleme fonksyonudur
        // POST: City/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityId,Name")] City city)
        {
            // Eğer gelen model doğru ise 
            if (ModelState.IsValid)
            {
                // yeni şehir ekle 
                _context.Add(city);
                //işlemleri kaydet 
                await _context.SaveChangesAsync();
                // Admin panel sayfasına yönlendirir bizi
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // Güncelleme sayfayı gösetermek için aşağıdaki fonksyon kullanılır 
        // GET: City/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Eğer gelen şehirin idisi null ise VerriBulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // bu id'ya ait olan şehir bilgilerini aranır 
            var city = await _context.cities.FindAsync(id);
            // eğer bir şehir bulnmadı ise Veribulunmadı mesajı gösterir
            if (city == null)
            {
                return NotFound();
            }
            //  şehir bilgilerini sayfaya yükler
            return View(city);
        }

        // Günceleme post fonksyonudur
        // POST: City/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CityId,Name")] City city)
        {
            // eğer bir şehir bulunmadı ise VeriBulunmadı mesajı gösterir 
            if (id != city.CityId)
            {
                return NotFound();
            }
            // eğer admin Update Butonua tıklarsa aaşğıdaki kodlar çalışacaktır
            if (ModelState.IsValid)
            {
                try
                {
                    // Güncelleme işlemini yapar
                    _context.Update(city);
                    //yapılan işlemleri kayderder
                    await _context.SaveChangesAsync();
                }
                // eğer güncelleme işleminde herhangi bir hata çıkarsa yakalar ve hata mesajı gösteiri
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // aynı Index sayfaya bizi yönlendirir
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // Silme sayfasını gösterir
        // GET: City/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // eğer gelen id unll ise Veribulunmadı mesjaını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // Şehir'e ait bilgileri değişkene ekler 
            var city = await _context.cities
                .FirstOrDefaultAsync(m => m.CityId == id);
            // eğer bir şehir bulunmadı ise veri bulunmadı mesajını gösterir
            if (city == null)
            {
                return NotFound();
            }
            //şehir modelini sayfaya yükler 
            return View(city);
        }

        // Eğer admin Sil butonuna tıklarsa aşaığdaki fonksyon çalışacaktır 
        // POST: City/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Şehirin bilgilerin arar ve bulur 
            var city = await _context.cities.FindAsync(id);
            // Şehir bilgilerin veritabanından siler
            _context.cities.Remove(city);
            // işlemi kayderder
            await _context.SaveChangesAsync();
            // Index sayfasına yönlendirir
            return RedirectToAction(nameof(Index));
        }

        // Şehir bulundu ise True göndriri - değil ise False 
        private bool CityExists(int id)
        {
            return _context.cities.Any(e => e.CityId == id);
        }
    }
}
