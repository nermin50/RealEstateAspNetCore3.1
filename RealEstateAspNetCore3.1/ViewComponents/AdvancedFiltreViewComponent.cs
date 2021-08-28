using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.ViewComponents
{
    public class AdvancedFiltreViewComponent : ViewComponent
    {
        private readonly DataContext _db;

        public AdvancedFiltreViewComponent(DataContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.citylist = new SelectList(CityGet(), "CityId", "Name");
            ViewBag.statuslist = new SelectList(statusGet(), "StatusId", "StatusName");

            return View();

        }
        public List<City> CityGet()
        {
            List<City> cities = _db.cities.ToList();
            return cities;
        }
        public List<Status> statusGet()
        {
            List<Status> statuses = _db.Status.ToList();
            return statuses;
        }
    }
}
