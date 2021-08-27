using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAspNetCore3._1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.ViewComponents
{
    public class StatusNameOneViewComponent : ViewComponent
    {
        private readonly DataContext _db;

        public StatusNameOneViewComponent(DataContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var statusname1 = _db.Tips.Where(i => i.StatusId == 1).Include(m => m.Status).FirstOrDefault();
            return View(statusname1);

        }
    }
}
