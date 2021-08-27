using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.ViewComponents
{
    public class StatusNameTwoViewComponent : ViewComponent
    {
        private readonly DataContext _db;

        public StatusNameTwoViewComponent(DataContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Satlık veriyi çağıryor 
            var statusname1 = _db.Tips.Where(i => i.StatusId == 2).Include(m => m.Status).FirstOrDefault();

            return View(statusname1);

        }
    }
}
