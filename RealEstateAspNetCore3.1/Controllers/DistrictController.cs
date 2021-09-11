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
    // yetkilendirem işlemi aşağıdaki fonksyonlar sadece admin tarafından kullanılır
    [Authorize(Roles = ("admin"))]
    public class DistrictController : Controller
    {
        // veitabanına bağlanamk için aşağıdaki değişkeni kullanyoruz
        private readonly DataContext _context;

        // Semt Konstraktörü
        public DistrictController(DataContext context)
        {
            _context = context;
        }

        /// Semt tablosundaki verileri listeleme işlemidir  
        // GET: District
        public async Task<IActionResult> Index()
        {
            //Semtleri veritabanındaki tablodan alıp değişkene yükler
            var dataContext = _context.districts.Include(d => d.City);
            //Verileri sayfaya gönderirir
            return View(await dataContext.ToListAsync());
        }

        // aşağıdaki fonksyon  Her semt detaylarını göstermek için kullanılır
        // GET: District/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // eğer gelen id null ise veri bulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // Semtin tüm bilgilerini getirir
            var district = await _context.districts
                .Include(d => d.City)
                .FirstOrDefaultAsync(m => m.DistrictId == id);
            // Eğer semt null ise veribulunmadı mesajını gösterir
            if (district == null)
            {
                return NotFound();
            }
            // Semt modelini sayfaya yükler 
            return View(district);
        }

        // Semt ekleme sayfasını gösetermek için aşağıdaki Get Fonksyonu kullanılır 
        // GET: District/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.cities , "CityId", "Name");
            return View();
        }

        // Semt ekleme fonksyonudur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DistrictId,DistrictName,CityId")] District district)
        {
            // Eğer gelen model doğru ise 
            if (ModelState.IsValid)
            {
                // Yeni Semt ekle 
                _context.Add(district);
                // yapılan işlemleri kaydet
                await _context.SaveChangesAsync();
                // Index sayfasına yönlendirir
                return RedirectToAction(nameof(Index));
            }
            // Semt ve Şehirin arasınaki Many to one ilişki olduğu için semtleri listelediğimizde Şehirleri listelememiz gerekiyor
            ViewData["CityId"] = new SelectList(_context.cities, "CityId", "Name", district.CityId);
            return View(district);
        }

        // Güncelleme sayfayı gösetermek için aşağıdaki fonksyon kullanılır 
        // GET: District/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Eğer gelen semtin idisi null ise VerriBulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // bu id'ya ait olan semt bilgilerini aranır 
            var district = await _context.districts.FindAsync(id);
            // eğer bir semt bulnmadı ise Veribulunmadı mesajı gösterir
            if (district == null)
            {
                return NotFound();
            }
            // Semt ve Şehirin arasınaki Many to one ilişki olduğu için semtleri listelediğimizde Şehirleri listelememiz gerekiyor
            ViewData["CityId"] = new SelectList(_context.cities, "Name", "Name", district.CityId);
            //Semt modelini sayfaya yükler
            return View(district);
        }

        // Günceleme post fonksyonudur
        // POST: District/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DistrictId,DistrictName,CityId")] District district)
        {
            // eğer bir semt bulunmadı ise VeriBulunmadı mesajı gösterir 
            if (id != district.DistrictId)
            {
                return NotFound();
            }
            // eğer admin tarafından gelen model  doğru ise aaşğıdaki kodlar çalışacaktır
            if (ModelState.IsValid)
            {
                try
                {
                    // Güncelleme işlemini yapar
                    _context.Update(district);
                    // yapılan işlemleri kaydeder
                    await _context.SaveChangesAsync();
                }
                // eğer güncelleme işleminde herhangi bir hata çıkarsa yakalar ve hata mesajı gösteiri
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(district.DistrictId))
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
            ViewData["CityId"] = new SelectList(_context.cities, "CityId", "CityId", district.CityId);
            return View(district);
        }

        // Silme sayfasını gösterir
        // GET: District/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // eğer gelen id unll ise Veribulunmadı mesjaını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // Semt'e ait bilgileri değişkene ekler 
            var district = await _context.districts
                .Include(d => d.City)
                .FirstOrDefaultAsync(m => m.DistrictId == id);
            // eğer bir Semt bulunmadı ise veri bulunmadı mesajını gösterir
            if (district == null)
            {
                return NotFound();
            }
            //Semt modelini sayfaya yükler 
            return View(district);
        }

        // Eğer admin Sil butonuna tıklarsa aşaığdaki fonksyon çalışacaktır 
        // POST: District/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Semtin bilgilerini arar ve bulur 
            var district = await _context.districts.FindAsync(id);
            // Semt bilgilerin veritabanından siler
            _context.districts.Remove(district);
            // yapılan işlemleri kaydeder
            await _context.SaveChangesAsync();
            // Index sayfasına yönlendirir
            return RedirectToAction(nameof(Index));
        }
        // Semt bulundu ise True göndriri - değil ise False 
        private bool DistrictExists(int id)
        {
            return _context.districts.Any(e => e.DistrictId == id);
        }
    }
}
