using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RealEstateAspNetCore3._1.Identity;

namespace RealEstateAspNetCore3._1.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        // user manager : application use'in özeliklerini üzerinden kullanabiliriz 
        private readonly UserManager<ApplicationUser> _userManager;
        // sign manager : bir oturum açmak için kullanılır 
        private readonly SignInManager<ApplicationUser> _signInManager;
        // logger : işlemeri bir log(sicil ) içinden kayderder 
        private readonly ILogger<LoginModel> _logger;


        // Login Konstruktoru :
        #region Constructor 
        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager)
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


        #region Login Model'i 
        public class InputModel
        {
            [Required]
          
            public string UserName { get; set; }

            [Required]
           // [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        #endregion


        // Login Get : bu fonksiyon üzerinden Login sayafsını açar sadece 
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        // Login Post . Login buttonua tıklarsak bu fonksyon çalışır 

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Login'in yapıldığı sayfaının linki 
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Login işlemi yapar Result true ise giriş yapar değil ise yapmaz 
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    // User giriş işlemin uyarısını gösterir
                    _logger.LogInformation("User logged in.");
                    // giriş yapıldığı sayfaya yönlendir 
                    return LocalRedirect(returnUrl);
                }
               
                else
                {
                    //Login işlemi başarısız ise bu mesaji gösteirir
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // Her hangi bir hata oluştu ise ayni sayfayı aç 
            return Page();
        }
    }
}
