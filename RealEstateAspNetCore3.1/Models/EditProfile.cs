using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Models
{
    public class EditProfile
    {
        public string id { get; set; }
        [Required]
        [DisplayName("Adi")]
        [StringLength(14, MinimumLength = 3, ErrorMessage = "Min 3 Max 14")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Soyadi")]
        public string Surname { get; set; }
        [Required]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Geçerli Email giriniz")]
        public string Email { get; set; }
        [Required]
        [DisplayName("KullanıcıAdi")]
        [StringLength(14, MinimumLength = 3, ErrorMessage = "Min 3 Max 14")]
        public string Username { get; set; }
    }
}
