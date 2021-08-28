using Microsoft.AspNetCore.Mvc;
using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly DataContext _db;

        public SliderViewComponent(DataContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var adv = _db.advertisements.ToList().Take(5);
            var imgs = _db.advPhotos.ToList();
            ViewBag.imgs = imgs;
            return View(adv);

        }
    }
}
