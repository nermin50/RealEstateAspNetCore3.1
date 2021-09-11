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
    public class NeighborhoodController : Controller
    {
        // veitabanına bağlanamk için aşağıdaki değişkeni kullanyoruz
        private readonly DataContext _context;
        // Mahalla Konstraktörü
        public NeighborhoodController(DataContext context)
        {
            _context = context;
        }
        /// Mahalla tablosundaki verileri listeleme işlemidir  
        // GET: Neighborhood
        public async Task<IActionResult> Index()
        {            
            //Mahallarları  veritabanındaki  tablodan alıp değişkene yükler
            var dataContext = _context.neighborhoods.Include(n => n.District).Include(m => m.District.City);
            return View(await dataContext.ToListAsync());
        }

        // aşağıdaki  fonksyon  Her semt detaylarını göstermek için kullanılır
        // GET: Neighborhood/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // eğer gelen id null ise veri bulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // Mahllanın tüm bilgilerini getirir
            var neighborhood = await _context.neighborhoods
                .Include(n => n.District)
                .FirstOrDefaultAsync(m => m.NeighborhoodId == id);
            // Eğer mahalla null ise veribulunmadı mesajını gösterir
            if (neighborhood == null)
            {
                return NotFound();
            }
            // mahalla modelini sayfaya yükler 
            return View(neighborhood);
        }
        // Mahalla ekleme sayfasını gösetermek için aşağıdaki Get Fonksyonu kullanılır 
        // GET: Neighborhood/Create
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.districts, "DistrictId", "DistrictName");
            return View();
        }

        // POST: Neighborhood/Create
        // Mahalla ekleme fonksyonudur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NeighborhoodId,NeighborhoodName,DistrictId")] Neighborhood neighborhood)
        {
            // Eğer gelen model doğru ise 
            if (ModelState.IsValid)
            {
                // Mahallayı ekle
                _context.Add(neighborhood);
                //yapılan işlemi kaydet
                await _context.SaveChangesAsync();
                // index sayfasına yönlendir 
                return RedirectToAction(nameof(Index));
            }
            // Semt ve Mahalla arasınaki Many to one ilişki olduğu için semtleri listelediğimizde Şehirleri listelememiz gerekiyor
            ViewData["DistrictId"] = new SelectList(_context.districts, "DistrictId", "DistrictName", neighborhood.DistrictId);
            return View(neighborhood);
        }
        // Güncelleme sayfayı gösetermek için aşağıdaki fonksyon kullanılır 
        // GET: Neighborhood/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Eğer gelen Mahallanın idisi null ise VerriBulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // bu id'ya ait olan mahalla bilgilerini aranır 
            var neighborhood = await _context.neighborhoods.FindAsync(id);
            // eğer bir mahalla bulnmadı ise Veribulunmadı mesajı gösterir
            if (neighborhood == null)
            {
                return NotFound();
            }
            // Semt ve Mahalla arasınaki Many to one ilişki olduğu için semtleri listelediğimizde Şehirleri listelememiz gerekiyor
            ViewData["DistrictId"] = new SelectList(_context.districts, "DistrictId", "DistrictName", neighborhood.DistrictId);
            return View(neighborhood);
        }

        // Aşaığdaki fonksyon Günceleme post fonksyonudur
        // POST: Neighborhood/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NeighborhoodId,NeighborhoodName,DistrictId")] Neighborhood neighborhood)
        {
            // eğer bir semt bulunmadı ise VeriBulunmadı mesajı gösterir 
            if (id != neighborhood.NeighborhoodId)
            {
                return NotFound();
            }
            // eğer admin tarafından gelen model  doğru ise aaşğıdaki kodlar çalışacaktır
            if (ModelState.IsValid)
            {
                try
                {   // Güncelleme işlemini yapar
                    _context.Update(neighborhood);
                    // yapılan işlemleri kayderder
                    await _context.SaveChangesAsync();
                }
                // eğer güncelleme işleminde herhangi bir hata çıkarsa yakalar ve hata mesajı gösteiri
                catch (DbUpdateConcurrencyException)
                {
                    if (!NeighborhoodExists(neighborhood.NeighborhoodId))
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
            // Semt ve Mahalla arasınaki Many to one ilişki olduğu için semtleri listelediğimizde Şehirleri listelememiz gerekiyor
            ViewData["DistrictId"] = new SelectList(_context.districts, "DistrictId", "DistrictId", neighborhood.DistrictId);
            return View(neighborhood);
        }
        // Silme sayfasını gösterir
        // GET: Neighborhood/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // eğer gelen id unll ise Veribulunmadı mesjaını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // Mahallaya ait bilgileri değişkene ekler 
            var neighborhood = await _context.neighborhoods
                .Include(n => n.District)
                .FirstOrDefaultAsync(m => m.NeighborhoodId == id);
            // eğer bir Mahalla bulunmadı ise veri bulunmadı mesajını gösterir
            if (neighborhood == null)
            {
                return NotFound();
            }
            //Mahalla modelini sayfaya yükler 
            return View(neighborhood);
        }

        // Eğer admin Sil butonuna tıklarsa aşaığdaki fonksyon çalışacaktır 
        // POST: Neighborhood/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Mahallanın bilgilerini arar ve bulur
            var neighborhood = await _context.neighborhoods.FindAsync(id);
            // Silem işlemi yapılır
            _context.neighborhoods.Remove(neighborhood);
            // yapılan işlemleri kaydeder
            await _context.SaveChangesAsync();
            //Index Sayfasına yönlendirir
            return RedirectToAction(nameof(Index));
        }
        // Eğer mahalla bulundu ise True - yoksa false gönderir
        private bool NeighborhoodExists(int id)
        {
            return _context.neighborhoods.Any(e => e.NeighborhoodId == id);
        }
    }
}
