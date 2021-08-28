using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace RealEstateAspNetCore3._1.ViewComponents
{
    public class AdvListViewComponent : ViewComponent
    {
        private readonly DataContext _db;

        public AdvListViewComponent(DataContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var imgs = _db.advPhotos.ToList();
            ViewBag.imgs = imgs;

            var adv = _db.advertisements.Include(l => l.Neighborhood).Include(n => n.Neighborhood.District).
                Include(m => m.Neighborhood.District.City).Include(e => e.Tip).Include(e => e.Tip.Status).OrderByDescending(i => i.AdvId); ;
            //ModelState.Clear();
            return View(adv.ToList().ToPagedList(page , 6 ));
        }
    }
}
