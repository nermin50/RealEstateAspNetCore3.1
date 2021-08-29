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
    [Authorize(Roles = ("admin"))]
    public class NeighborhoodController : Controller
    {
        private readonly DataContext _context;

        public NeighborhoodController(DataContext context)
        {
            _context = context;
        }

        // GET: Neighborhood
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.neighborhoods.Include(n => n.District).Include(m => m.District.City);
            return View(await dataContext.ToListAsync());
        }

        // GET: Neighborhood/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neighborhood = await _context.neighborhoods
                .Include(n => n.District)
                .FirstOrDefaultAsync(m => m.NeighborhoodId == id);
            if (neighborhood == null)
            {
                return NotFound();
            }

            return View(neighborhood);
        }

        // GET: Neighborhood/Create
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.districts, "DistrictId", "DistrictName");
            return View();
        }

        // POST: Neighborhood/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NeighborhoodId,NeighborhoodName,DistrictId")] Neighborhood neighborhood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(neighborhood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.districts, "DistrictId", "DistrictName", neighborhood.DistrictId);
            return View(neighborhood);
        }

        // GET: Neighborhood/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neighborhood = await _context.neighborhoods.FindAsync(id);
            if (neighborhood == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.districts, "DistrictId", "DistrictName", neighborhood.DistrictId);
            return View(neighborhood);
        }

        // POST: Neighborhood/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NeighborhoodId,NeighborhoodName,DistrictId")] Neighborhood neighborhood)
        {
            if (id != neighborhood.NeighborhoodId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(neighborhood);
                    await _context.SaveChangesAsync();
                }
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.districts, "DistrictId", "DistrictId", neighborhood.DistrictId);
            return View(neighborhood);
        }

        // GET: Neighborhood/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neighborhood = await _context.neighborhoods
                .Include(n => n.District)
                .FirstOrDefaultAsync(m => m.NeighborhoodId == id);
            if (neighborhood == null)
            {
                return NotFound();
            }

            return View(neighborhood);
        }

        // POST: Neighborhood/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var neighborhood = await _context.neighborhoods.FindAsync(id);
            _context.neighborhoods.Remove(neighborhood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NeighborhoodExists(int id)
        {
            return _context.neighborhoods.Any(e => e.NeighborhoodId == id);
        }
    }
}
