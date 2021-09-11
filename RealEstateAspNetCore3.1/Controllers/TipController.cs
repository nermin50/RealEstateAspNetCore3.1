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
    public class TipController : Controller
    {
        // veitabanına bağlanamk için aşağıdaki değişkeni kullanyoruz
        private readonly DataContext _context;
        //Tip Konstraktörü 
        public TipController(DataContext context)
        {
            _context = context;
        }

        /// Durum tablosundaki verileri listeleme işlemidir  
        // GET: Tip
        public async Task<IActionResult> Index()
        {
            //Tipleri  veritabanındaki  tablodan alıp değişkene yükler
            var dataContext = _context.Tips.Include(t => t.Status);
            return View(await dataContext.ToListAsync());
        }
        // aşağıdaki  fonksyon  Her Tip detaylarını göstermek için kullanılır
        // GET: Tip/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // eğer gelen id null ise veri bulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // Tipin tüm bilgilerini getirir
            var tip = await _context.Tips
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.TypeId == id);
            // Eğer Tip  null ise veribulunmadı mesajını gösterir
            if (tip == null)
            {
                return NotFound();
            }
            // Tip modelini sayfaya yükler
            return View(tip);
        }
        // Tip ekleme sayfasını gösetermek için aşağıdaki Get Fonksyonu kullanılır 
        // GET: Tip/Create
        public IActionResult Create()
        {
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName");
            return View();
        }

        // POST: Tip/Create
        // Tip ekleme fonksyonudur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,TypeName,StatusId")] Tip tip)
        {
            // Eğer gelen model doğru ise 
            if (ModelState.IsValid)
            {
                // Tip verisini veritabanına
                _context.Add(tip);
                // yapılan işlemleri kaydet
                await _context.SaveChangesAsync();
                // Index Sayfasına yönlendir 
                return RedirectToAction(nameof(Index));
            }
            // Durum ve Tip arasınaki Many to one ilişki olduğu için semtleri listelediğimizde Şehirleri listelememiz gerekiyor
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusId", tip.StatusId);
            return View(tip);
        }
        // Güncelleme sayfayı gösetermek için aşağıdaki fonksyon kullanılır 
        // GET: Tip/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Eğer gelen Tipin idisi null ise VerriBulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // bu id'ya ait olan Tip bilgilerini aranır 
            var tip = await _context.Tips.FindAsync(id);
               // eğer bir Tip bulnmadı ise Veribulunmadı mesajı gösterir
            if (tip == null)
            {
                return NotFound();
            }
            // Tip  ve Durum arasınaki Many to one ilişki olduğu için semtleri listelediğimizde Şehirleri listelememiz gerekiyor
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", tip.StatusId);
            return View(tip);
        }

        // POST: Tip/Edit/5
        // Aşaığdaki fonlsyon Günceleme post fonksyonudur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,TypeName,StatusId")] Tip tip)
        {
            // eğer bir semt bulunmadı ise VeriBulunmadı mesajı gösterir 
            if (id != tip.TypeId)
            {
                return NotFound();
            }
            // eğer admin tarafından gelen model  doğru ise aaşğıdaki kodlar çalışacaktır
            if (ModelState.IsValid)
            {
                try
                {
                    // Güncelleme işlemini yapar
                    _context.Update(tip);
                    // Yapılan işlemleri kaydeder
                    await _context.SaveChangesAsync();
                }
                // eğer güncelleme işleminde herhangi bir hata çıkarsa yakalar ve hata mesajı gösteiri
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipExists(tip.TypeId))
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
            // Tip  ve Durum arasınaki Many to one ilişki olduğu için semtleri listelediğimizde Şehirleri listelememiz gerekiyor
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusId", tip.StatusId);
            return View(tip);
        }
        // Silme sayfasını gösterir
        // GET: Tip/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // eğer gelen id unll ise Veribulunmadı mesjaını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // Tipe ait bilgileri değişkene ekler 
            var tip = await _context.Tips
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.TypeId == id);
            // eğer bir Tip verisi bulunmadı ise veri bulunmadı mesajını gösterir
            if (tip == null)
            {
                return NotFound();
            }
            //Tip modelini sayfaya yükler 
            return View(tip);
        }

        // POST: Tip/Delete/5
        // Eğer admin Sil butonuna tıklarsa aşaığdaki fonksyon çalışacaktır 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Tipin bilgilerini arar ve bulur
            var tip = await _context.Tips.FindAsync(id);
            // Silem işlemi yapılır
            _context.Tips.Remove(tip);
            // yapılan işlemi kaydeder
            await _context.SaveChangesAsync();
            // Index Sayfasına Yönlendirir 
            return RedirectToAction(nameof(Index));
        }

        // Eğer Bir tip bulundu ise True Değil ise False Gönderir 
        private bool TipExists(int id)
        {
            return _context.Tips.Any(e => e.TypeId == id);
        }
    }
}
