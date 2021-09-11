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
        // identity veritabanına bağlana kodu 
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


        
        // veritabanındaki bulunan bütün userleri listeleme fonksyonudur 
        public ActionResult UserList()
        {
            var u = _userManager.Users;
            return View(u);

        }
        // aşağıdaki fonksyon  Şifre resetleme sayfasını göstermek için kullanılır  
        public ActionResult UpdatePassword()
        {
            return View();
        }
        // Kullanıcı resetleme butonuna tıkladıktan sonra aşağıdaki fonksyon çalışacaktır
        [HttpPost]
        public ActionResult UpdatePassword(UpdatePassword model)
        {
            // eğer kullanıcı  tüm gerekli olan inputları dodurursa aşağıdaki işlemler yapılacaktır 
            if (ModelState.IsValid)
            {
                // Giriş yapan kullanıcı idisini  userid değişkene yükler
                var userid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                // kullanıcının tüm bilgilerini id üzerineden bulur 
                var user1 = _userManager.FindByIdAsync(userid);
                // ondan sonra CahngedPassword üzerinden şifre değişikliğini yapar 
                var user = _userManager.ChangePasswordAsync(user1.Result, model.OldPassword, model.NewPassword);
                //yukarıda yapılan  işlemleri kaydeder 
                _context.SaveChangesAsync();
                //  işlem başaryla tamamlalndı  sayfasına yönlendirir
                return View("UpdateProfileSuccess");
            }
            // Eğer hiç bir işlem yapılamza aynı modeli aynı sayfaya  yükler 
            return View(model);
        }
      
        //Get 
        // Profil güncelleme sayfasını gösterir
        public ActionResult Profile()
        {
            // Aşağıdaki işlemler giriş yapan kullanıcının bilgilerin veritabanından yükler 
           var userid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.FindByIdAsync(userid);
            // gelen bilgileri EditProfile nesneye yükler 
            var data = new EditProfile()
            {
                id = user.Result.Id,
                Name = user.Result.Name,
                Surname = user.Result.Surname,
                Username = user.Result.UserName,
            Email = user.Result.Email,
            };
            // ondan sonra bilgileri sayfaya  yükler 
            return View(data);
        }

        // Eğer kullancı güncelleme işlemini yaprasa aşağıdaki fonksyon çalışacaktır 
        [HttpPost]
        public ActionResult Profile(EditProfile model)
        {
            // Kullanıcının bilgilerin veritabanından yükler değişkene 
            string userId = _userManager.GetUserId(User);;
            var user = _userManager.FindByIdAsync(userId);
           
            // eski bilgileri yeni bilgiler ile değiştirilir 
            user.Result.Name = model.Name;
            user.Result.UserName = model.Username;
            user.Result.Surname = model.Surname;
            user.Result.Email = model.Email;
            // yukarıda yapılan güncelleme işlemini yapar 
            _userManager.UpdateAsync(user.Result);
            // bütün işlemleri kayderder 
            _context.SaveChangesAsync();
            // işlemi yaptıktan sonra  işlem başaryla tamamlalndı  sayfasına yönlendirir
            return View("UpdateProfileSuccess");

        }
        // aşağıdaki fonksyon işlem başaryla tamamlalndı mesaji göstermek için kullanılır 
        public IActionResult UpdateProfileSuccess()
        {
            return View();
        }

        // Bu fonskyon Kullanılmadı ....
        public IActionResult Index()
        {
            return View();
        }


    }
}
