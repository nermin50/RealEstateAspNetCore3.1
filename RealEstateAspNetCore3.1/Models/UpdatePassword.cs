using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Models
{
    public class UpdatePassword
    {
        [Required]
        [DisplayName("Eski Password")]
        public string OldPassword { get; set; }
        [Required]
        [DisplayName("Yeni Password")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "En az 5 karekter olmalı")]
        public string NewPassword { get; set; }
        [Required]
        [DisplayName("Tekrar Yeni Password")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "En az 5 karekter olmalı")]
        [Compare("NewPassword", ErrorMessage = "Şifreler ayni değil")]
        public string ConNewPassword { get; set; }
    }
}
