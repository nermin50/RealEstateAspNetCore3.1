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
    public class FooterLinkController : Controller
    {
        // veitabanına bağlanamk için aşağıdaki değişkeni kullanyoruz
        private readonly DataContext _context;

        // Controllerin konstraktörü 
        public FooterLinkController(DataContext context)
        {
            _context = context;
        }
        /// Footer Link tablosundaki verileri listeleme işlemidir  
        // GET: FooterLink
        public async Task<IActionResult> Index()
        {
            //Footer linkleri  veritabanındaki  tablodan alıp değişkene yükler
            return View(await _context.footerLinks.ToListAsync());
        }
        // aşağıdaki  fonksyon  footer link detaylarını göstermek için kullanılır
        // GET: FooterLink/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // eğer gelen id null ise veri bulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // footerLink tüm bilgilerini getirir
            var footerLink = await _context.footerLinks
                .FirstOrDefaultAsync(m => m.FooterId == id);
            // Eğer footerLink null ise veribulunmadı mesajını gösterir
            if (footerLink == null)
            {
                return NotFound();
            }
            // footerLink modelini sayfaya yükler 
            return View(footerLink);
        }

        // Güncelleme sayfayı gösetermek için aşağıdaki fonksyon kullanılır 
        // GET: FooterLink/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Eğer gelen footerLink idisi null ise VerriBulunmadı mesajını gösterir
            if (id == null)
            {
                return NotFound();
            }
            // bu id'ya ait olan footerLink bilgilerini aranır 
            var footerLink = await _context.footerLinks.FindAsync(id);
            // eğer bir footerLink bulnmadı ise Veribulunmadı mesajı gösterir
            if (footerLink == null)
            {
                return NotFound();
            }
            // footer link modelini sayfaya gönderir
            return View(footerLink);
        }

        // POST: FooterLink/Edit/5
        // Aşaığdaki fonksyon Günceleme post fonksyonudur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FooterId,facebook,twitter,instagram,youtube,googlemap,email,address,whatsapp")] FooterLink footerLink)
        {
            // eğer bir semt bulunmadı ise VeriBulunmadı mesajı gösterir 
            if (id != footerLink.FooterId)
            {
                return NotFound();
            }
            // eğer admin tarafından gelen model  doğru ise aaşğıdaki kodlar çalışacaktır
            if (ModelState.IsValid)
            {
                try
                {
                    // Güncelleme işlemin yapar 
                    _context.Update(footerLink);
                    // yapılan işlemi kaydeder
                    await _context.SaveChangesAsync();
                }
                // eğer güncelleme işleminde herhangi bir hata çıkarsa yakalar ve hata mesajı gösteiri
                catch (DbUpdateConcurrencyException)
                {
                    if (!FooterLinkExists(footerLink.FooterId))
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
            // footer link modelini sayfaya gönderir
            return View(footerLink);
        }

        // Eğer veri bulundu ise True bulunmadı ise false gönderir 
        private bool FooterLinkExists(int id)
        {
            return _context.footerLinks.Any(e => e.FooterId == id);
        }
    }
}
