using Microsoft.AspNetCore.Mvc;
using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.ViewComponents
{
    public class SearchPartialViewComponent : ViewComponent
    {
        private readonly DataContext _db;

        public SearchPartialViewComponent(DataContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();

        }

    }
}
