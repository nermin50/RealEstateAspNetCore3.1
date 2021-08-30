using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Models
{
    public class FooterLink
    {
        [Key]
        public int FooterId { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string instagram { get; set; }
        public string youtube { get; set; }
        public string googlemap { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string whatsapp { get; set; }

    }
}
