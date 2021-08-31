using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstateAspNetCore3._1.Models;

namespace RealEstateAspNetCore3._1.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly DataContext _context;
        private IHostingEnvironment Environment;

        public AdvertisementController(DataContext context , IHostingEnvironment _environment)
        {
            Environment = _environment;

            _context = context;
        }

        public ActionResult Images(int id)
        {
            var adv = _context.advertisements.Where(i => i.AdvId == id).ToList();
            var rsml = _context.advPhotos.Where(i => i.AdvId == id).ToList();

            ViewBag.rsml = rsml;
            ViewBag.adv = adv;

            return View();


        }
        [HttpPost]
        public ActionResult Images(int id, List<IFormFile> file)
        {
            if (file != null)
            {
                string wwwPath = this.Environment.WebRootPath;
                string contentPath = this.Environment.ContentRootPath;
              
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                List<string> uploadedFiles = new List<string>();
                foreach (IFormFile postedFile in file)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);
                    AdvPhoto rsm = new AdvPhoto();
                    rsm.AdvPhotoName = fileName.ToString();
                    // ilişkilendirme 
                    rsm.AdvId = id;
                    _context.advPhotos.Add(rsm);
                    _context.SaveChanges();
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                       
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    }
                }

                // Resim bu Modelin

                return RedirectToAction("Images");
            }

            ViewBag.alert = "Please insert Photo.....";
            return RedirectToAction("Index");

        }
        public ActionResult Deleteimages(int? id , int? advId)
        {
            if (id == null)
            {
                return BadRequest("request is incorrect");
            }
            AdvPhoto advphoto = _context.advPhotos.Find(id);
            if (advphoto == null)
            {
                return NotFound();
            }


            _context.advPhotos.Remove(advphoto);
            _context.SaveChanges();
            return RedirectToAction("Images" , new { id = advId });
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
            var username = User.Identity.Name;
            var dataContext = _context.advertisements.Where(i => i.UserName == username).Include(a => a.Neighborhood).Include(b => b.Neighborhood.District)
                .Include(c => c.Neighborhood.District.City).Include(n=> n.Tip).Include(m => m.Tip.Status);
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
            ViewBag.Name = User.Identity.Name;
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
                advertisement.UserName = User.Identity.Name;
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
