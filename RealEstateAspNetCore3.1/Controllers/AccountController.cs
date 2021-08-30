using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateAspNetCore3._1.Identity;
using RealEstateAspNetCore3._1.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace RealEstateAspNetCore3._1.Controllers
{
     
    [Authorize(Roles = ("admin,user"))] // admin ve user şifre ve profil güncellemesi yapabilir 
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


        
        public ActionResult UserList()
        {
            var u = _userManager.Users;
            return View(u);

        }
        public ActionResult UpdatePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdatePassword(UpdatePassword model)
        {
            if (ModelState.IsValid)
            {
                var userid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user1 = _userManager.FindByIdAsync(userid);
                var user = _userManager.ChangePasswordAsync(user1.Result, model.OldPassword, model.NewPassword);
                _context.SaveChangesAsync();
                return View("UpdateProfileSuccess");
            }
            return View(model);
        }
      
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
            string userId = _userManager.GetUserId(User);;
            var user = _userManager.FindByIdAsync(userId);
           
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
