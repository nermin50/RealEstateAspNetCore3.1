using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateAspNetCore3._1.Identity;
using RealEstateAspNetCore3._1.Models;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RealEstateAspNetCore3._1.Controllers
{
    public class AccountController : Controller
    {
        private readonly IdentityDataContext _context;

        // user manager : application use'in özeliklerini üzerinden kullanabiliriz 
        private readonly UserManager<ApplicationUser> _userManager;
      
        private readonly IHttpContextAccessor _httpContextAccessor;


        // Account Konstruktoru :
        #region Constructor 
        public AccountController(
            UserManager<ApplicationUser> userManager ,  IHttpContextAccessor httpContextAccessor , IdentityDataContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;

        }
        #endregion

        //Get 
        public ActionResult Profile()
        {
           var userid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.FindByIdAsync(userid);
            var data = new EditProfile()
            {
                id = user.Result.Id,
                Name = user.Result.Name,
                Surname = user.Result.Surname,
                Username = user.Result.UserName,
            Email = user.Result.Email,
            };
            return View(data);
        }
        [HttpPost]
        public ActionResult Profile(EditProfile model)
        {
            var user = _userManager.FindByIdAsync(model.id);
            user.Result.Name = model.Name;
            user.Result.UserName = model.Username;
            user.Result.Surname = model.Surname;
            user.Result.Email = model.Email;
            _userManager.UpdateAsync(user.Result);
            _context.SaveChangesAsync();
            return View("UpdateProfileSuccess");

        }
        public IActionResult UpdateProfileSuccess()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}
