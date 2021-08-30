using System.ComponentModel.DataAnnotations;

namespace RealEstateAspNetCore3._1.Models
{
    public class ContactUs
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }

    }
}
