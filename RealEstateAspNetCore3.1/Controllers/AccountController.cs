using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RealEstateAspNetCore3._1.Identity;
using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Controllers
{
    public class AccountController : Controller
    {
        // User Model'ine Ulaşamak için 
        private readonly UserManager<ApplicationUser> _userManager;
        // Role Modeline Ulaşmak için  
        private RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager , RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
       
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        /**********************************************/


        //**Register'de yeni Kayıt oluşturmak için kulllanılır*/

        [HttpPost]
        public IActionResult Register (Register model)
        {
            /*Model doğru yada doğru değil ise kontrol et*/
            if (ModelState.IsValid)
            {
                /*Yeni bir nesne oluştur */
                var user = new ApplicationUser();
                /*View'deki Name'i al = Database'kaydet */
                user.Name = model.Name;
                /*View'deki Username'i al = Database'kaydet */
                user.UserName = model.Username;
                /*View'deki Surname'i al = Database'kaydet */
                user.Surname = model.Surname;
                /*View'deki Email'i al = Database'kaydet */
                user.Email = model.Email;

                /*Yeni kayt oluştur  User ve Şifreyi burda Hash işlemini yap : Hash : Şifreleme*/
                var result =  _userManager.CreateAsync(user, model.Password).Result;

                /*Eğer kllanıcı başarılı ile oluşturludu */
                if (result.Succeeded)
                {
                    /*Bu user'e bir Role ver buda normal user*/
                    if (_roleManager.RoleExistsAsync("user").Result)
                    {
                        /**he zaman yeni kullanıcı normal user olarak kaydeder çünkü bir admin var sadcde kalanalar hepsi user*/
                        _userManager.AddToRoleAsync(user , "user");

                    }
                    /*Index sayfasına Git*/
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //**Eğer Kullancı Oluştuma aşamasında bir hata olursa bu mesaj çıkar*/
                    ModelState.AddModelError("RegisterUserError", "Kullanıccı oluşturma hatası");
                }
            }
            return View(model);
        }




    }
}
