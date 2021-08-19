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
    public class DistrictController : Controller
    {
        private readonly DataContext _context;

        public DistrictController(DataContext context)
        {
            _context = context;
        }

        // GET: District
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.districts.Include(d => d.City);
            return View(await dataContext.ToListAsync());
        }

        // GET: District/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.districts
                .Include(d => d.City)
                .FirstOrDefaultAsync(m => m.DistrictId == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // GET: District/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.cities, "CityId", "CityId");
            return View();
        }

        // POST: District/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DistrictId,DistrictName,CityId")] District district)
        {
            if (ModelState.IsValid)
            {
                _context.Add(district);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.cities, "CityId", "CityId", district.CityId);
            return View(district);
        }

        // GET: District/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.districts.FindAsync(id);
            if (district == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.cities, "CityId", "CityId", district.CityId);
            return View(district);
        }

        // POST: District/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DistrictId,DistrictName,CityId")] District district)
        {
            if (id != district.DistrictId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(district);
                    await _context.SaveChangesAsync();
                }
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.cities, "CityId", "CityId", district.CityId);
            return View(district);
        }

        // GET: District/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.districts
                .Include(d => d.City)
                .FirstOrDefaultAsync(m => m.DistrictId == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // POST: District/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var district = await _context.districts.FindAsync(id);
            _context.districts.Remove(district);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictExists(int id)
        {
            return _context.districts.Any(e => e.DistrictId == id);
        }
    }
}
