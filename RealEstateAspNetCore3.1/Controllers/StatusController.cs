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
    public class StatusController : Controller
    {
        // veitabanına bağlanamk için aşağıdaki değişkeni kullanyoruz
        private readonly DataContext _context;
        //Status Konstraktörü
        public StatusController(DataContext context)
        {
            _context = context;
        }
        /// Durum tablosundaki verileri listeleme işlemdir  
        // GET: Status
        public async Task<IActionResult> Index()
        {
            return View(await _context.Status.ToListAsync());
        }
        // aşağıdaki fonksyon  Her Durum detaylarını göstermek için kullanılır
        // GET: Status/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // eğer gelen id null ise veri bulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // Durumun tüm bilgilerini getirir
            var status = await _context.Status
                .FirstOrDefaultAsync(m => m.StatusId == id);
            // Eğer Durum null ise veribulunmadı mesajını gösterir
            if (status == null)
            {
                return NotFound();
            }
            // Durum modelini sayfaya yükler 
            return View(status);
        }
        // Durum ekleme sayfasını gösetermek için aşağıdaki Get Fonksyonu kullanılır 
        // GET: Status/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        // Mahalla ekleme post fonksyonudur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,StatusName")] Status status)
        {
            // Eğer gelen model doğru ise
            if (ModelState.IsValid)
            {
                // Durum ekle
                _context.Add(status);
                // yapılan işlemleri kaydet
                await _context.SaveChangesAsync();
                // Index Sayfasına yönlendir
                return RedirectToAction(nameof(Index));
            }
            return View(status);
        }
        // Güncelleme sayfayı gösetermek için aşağıdaki fonksyon kullanılır 
        // GET: Status/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Eğer gelen Mahallanın idisi null ise VerriBulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // bu id'ya ait olan durum bilgilerini aranır 
            var status = await _context.Status.FindAsync(id);
            // eğer bir durum bulnmadı ise Veribulunmadı mesajı gösterir
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }
        // Aşaığdaki fonlsyon Günceleme post fonksyonudur
        // POST: Status/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,StatusName")] Status status)
        {
            // eğer bir durum bulunmadı ise VeriBulunmadı mesajı gösterir 
            if (id != status.StatusId)
            {
                return NotFound();
            }
            // eğer admin tarafından gelen model  doğru ise aaşğıdaki kodlar çalışacaktır
            if (ModelState.IsValid)
            {
                try
                {
                    //Güncelleme işlemi yapar
                    _context.Update(status);
                    //Yapılan işlemleri kaydet
                    await _context.SaveChangesAsync();
                }
                // eğer güncelleme işleminde herhangi bir hata çıkarsa yakalar ve hata mesajı gösteiri
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusExists(status.StatusId))
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
            return View(status);
        }
        // Silme sayfasını gösterir
        // GET: Status/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // eğer gelen id unll ise Veribulunmadı mesjaını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // Duruma ait bilgileri değişkene ekler 
            var status = await _context.Status
                .FirstOrDefaultAsync(m => m.StatusId == id);
            // eğer bir Emlak durumu bulunmadı ise veri bulunmadı mesajını gösterir
            if (status == null)
            {
                return NotFound();
            }
            //Durum modelini sayfaya yükler
            return View(status);
        }
        // Eğer admin Sil butonuna tıklarsa aşaığdaki fonksyon çalışacaktır 
        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Durumun bilgilerini arar ve bulur
            var status = await _context.Status.FindAsync(id);
            //Silme işlemini yapar
            _context.Status.Remove(status);
            // Yapılan işlemleri kaydeder
            await _context.SaveChangesAsync();
            // Index Sayfasına yönlendirir
            return RedirectToAction(nameof(Index));
        }

        // eğer bir emlak durumu verisi bulundu ise True değil ise False gönderir
        private bool StatusExists(int id)
        {
            return _context.Status.Any(e => e.StatusId == id);
        }
    }
}
