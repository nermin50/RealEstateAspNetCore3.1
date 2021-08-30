using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstateAspNetCore3._1.Models;

namespace RealEstateAspNetCore3._1.Controllers
{
    public class FooterLinkController : Controller
    {
        private readonly DataContext _context;

        public FooterLinkController(DataContext context)
        {
            _context = context;
        }

        // GET: FooterLink
        public async Task<IActionResult> Index()
        {
            return View(await _context.footerLinks.ToListAsync());
        }

        // GET: FooterLink/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footerLink = await _context.footerLinks
                .FirstOrDefaultAsync(m => m.FooterId == id);
            if (footerLink == null)
            {
                return NotFound();
            }

            return View(footerLink);
        }

    
        // GET: FooterLink/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footerLink = await _context.footerLinks.FindAsync(id);
            if (footerLink == null)
            {
                return NotFound();
            }
            return View(footerLink);
        }

        // POST: FooterLink/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FooterId,facebook,twitter,instagram,youtube,googlemap,email,address,whatsapp")] FooterLink footerLink)
        {
            if (id != footerLink.FooterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(footerLink);
                    await _context.SaveChangesAsync();
                }
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
                return RedirectToAction(nameof(Index));
            }
            return View(footerLink);
        }

        private bool FooterLinkExists(int id)
        {
            return _context.footerLinks.Any(e => e.FooterId == id);
        }
    }
}
