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
    public class AdvertisementController : Controller
    {
        private readonly DataContext _context;

        public AdvertisementController(DataContext context)
        {
            _context = context;
        }

        public List<City> CityGet()
        {
            List<City> cities = _context.cities.ToList();
            return cities;
        }
        public ActionResult DistrictGet(int CityId)
        {
            List<District> districtlist = _context.districts.Where(x => x.CityId == CityId).ToList();
            ViewBag.districtListesi = new SelectList(districtlist, "DistrictId", "DistrictName");

            return PartialView("DistrictPartial");
        }

        public ActionResult NgbhdGet(int districtid)
        {
            List<Neighborhood> neighborhoodlist = _context.neighborhoods.Where(x => x.DistrictId == districtid).ToList();
            ViewBag.nghbdlistesi = new SelectList(neighborhoodlist, "NeighborhoodId", "NeighborhoodName");

            return PartialView("NgbhdPartial");
        }

        public List<Status> statusGet()
        {
            List<Status> statuses = _context.Status.ToList();
            return statuses;
        }
        public ActionResult typeGet(int statusid)
        {
            // because status sub table of Type 
            List<Tip> typelist = _context.Tips.Where(x => x.StatusId == statusid).ToList();
            ViewBag.typelistesi = new SelectList(typelist, "TypeId", "TypeName");


            return PartialView("TypePartial");
        }



        // GET: Advertisement
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.advertisements.Include(a => a.Neighborhood).Include(n=> n.Tip);
            return View(await dataContext.ToListAsync());
        }

        // GET: Advertisement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.advertisements
                .Include(a => a.Neighborhood)
                .FirstOrDefaultAsync(m => m.AdvId == id);
            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }

        // GET: Advertisement/Create
        public IActionResult Create()
        {
            //for list city and district to create page
            ViewBag.citylist = new SelectList(CityGet(), "CityId", "Name");

            ViewBag.statuslist = new SelectList(statusGet(), "StatusId", "StatusName"); return View();
        }

        // POST: Advertisement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdvId,Description,Price,NumOfRoom,NumOfBath,Credit,Area,Floor,Feature,Telephone,Addres,CityId,DistrictId,StatusId,NeighborhoodId,TypeId")] Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(advertisement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NeighborhoodId"] = new SelectList(_context.neighborhoods, "NeighborhoodId", "NeighborhoodId", advertisement.NeighborhoodId);
            return View(advertisement);
        }

        // GET: Advertisement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.advertisements.FindAsync(id);
            if (advertisement == null)
            {
                return NotFound();
            }
            ViewData["NeighborhoodId"] = new SelectList(_context.neighborhoods, "NeighborhoodId", "NeighborhoodId", advertisement.NeighborhoodId);
            return View(advertisement);
        }

        // POST: Advertisement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdvId,Description,Price,NumOfRoom,NumOfBath,Credit,Area,Floor,Feature,Telephone,Addres,CityId,DistrictId,StatusId,NeighborhoodId,TypeId")] Advertisement advertisement)
        {
            if (id != advertisement.AdvId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advertisement);
                    await _context.SaveChangesAsync();
                }
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["NeighborhoodId"] = new SelectList(_context.neighborhoods, "NeighborhoodId", "NeighborhoodId", advertisement.NeighborhoodId);
            return View(advertisement);
        }

        // GET: Advertisement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.advertisements
                .Include(a => a.Neighborhood)
                .FirstOrDefaultAsync(m => m.AdvId == id);
            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }

        // POST: Advertisement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertisement = await _context.advertisements.FindAsync(id);
            _context.advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertisementExists(int id)
        {
            return _context.advertisements.Any(e => e.AdvId == id);
        }
    }
}
