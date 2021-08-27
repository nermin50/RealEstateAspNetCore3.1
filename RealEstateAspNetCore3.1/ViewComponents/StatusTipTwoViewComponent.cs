using Microsoft.AspNetCore.Mvc;
using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.ViewComponents
{
    public class StatusTipTwoViewComponent : ViewComponent
    {
        private readonly DataContext _db;

        public StatusTipTwoViewComponent(DataContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tiplist = _db.Tips.Where(i => i.StatusId == 2).ToList();
            return View(tiplist);

        }
    }
}
