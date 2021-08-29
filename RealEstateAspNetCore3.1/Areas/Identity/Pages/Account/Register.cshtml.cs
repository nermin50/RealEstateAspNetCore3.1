using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RealEstateAspNetCore3._1.Identity;

namespace RealEstateAspNetCore3._1.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        // sign manager : bir oturum açmak için kullanılır
        private readonly SignInManager<ApplicationUser> _signInManager;
        // user manager : application use'in özeliklerini üzerinden kullanabiliriz 
        private readonly UserManager<ApplicationUser> _userManager;
        // logger : işlemeri bir log(sicil ) içinden kayderder 
        private readonly ILogger<RegisterModel> _logger;

        #region Register'in konstraktoru 
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
          
        }
        #endregion


        #region Yardımcı değişkenler 
        [BindProperty]
        // inputten gelene veriyi tutar
        public InputModel Input { get; set; }
        // eğer harici giriş kullancaksak : facebook , twitter ... vs 
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        // oturum açıldığı sayfaının linkini tutar : yani eğer kullanıcı ilanın sayfasından giriş yaparsa
        // / giriş yaptıktan sonra ayni ilan sayfasına yönlendirir 
        public string ReturnUrl { get; set; }
        // hata mesaji 
        [TempData]
        public string ErrorMessage { get; set; }
        #endregion

        // Registir Modeli
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
         
            [Display(Name = "UserName")]
            public string UserName { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
           // [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

          //  [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        // Register html sayfasını  Getir
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        // Registr post : Register Buttonuna tıkladığımızda bu fonksyon çalışır 
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Register'in yapıldığı sayfaının linki 
            returnUrl = returnUrl ?? Url.Content("~/");
            // Harici linkten   kayt oluşturursak : facebook twitter üzerinden 
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //Yeni application oluştur
                var user = new ApplicationUser { UserName = Input.UserName, Email = Input.Email };
                //  kullanıcının şifresine Hash (şifreleme ) yapar : şifreyi teextten Hexadecimal'e çevirir 
                var result = await _userManager.CreateAsync(user, Input.Password);
                // eğer oluşrurmada bir hata çıklazsa true 
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "user").Wait();
                    //kaut oluşturludu 
                    _logger.LogInformation("User created a new account with password.");
                    //Giriş Yap 
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // sayfaya yönlendir 
                    return LocalRedirect(returnUrl);

                }
                // eğer bir hata oluştu ise bu hatayı göster : birden fazla hata varsa hepsni listeler
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Her hangi bir hata oluştu ise ayni sayfayı aç 
            return Page();
        }
    }
}
