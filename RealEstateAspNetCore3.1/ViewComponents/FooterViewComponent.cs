using Microsoft.AspNetCore.Mvc;
using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {

        private readonly DataContext _db;

        public FooterViewComponent(DataContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footer = _db.footerLinks.ToList();

            return View(footer);
        }
    }
}
