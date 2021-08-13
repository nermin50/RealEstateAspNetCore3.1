using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Burda Microsoftun Idenitiy User Class'ini hazır olarak kullandık 
        //bütün Idenitiy User'deki özelikleri bu sınıf üzerinden kullanabiliriz
        // iki tane değişken fazla ekledik - burada eklenen değişkenler User Tablosuna eklenecek ve istedğimiz değişkenleri ekleyebiliriz 
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
