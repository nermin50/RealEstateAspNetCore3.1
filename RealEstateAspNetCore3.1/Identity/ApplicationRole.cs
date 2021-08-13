using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Identity
{
    public class ApplicationRole : IdentityRole
    {

        // Burda Microsoftun Idenitiy Role Class'ini hazır olarak kullandık 
        //bütün Idenitiy Role'deki özelikleri bu sınıf üzerinden kullanabiliriz
        // iki tane değişken fazla ekledik - burada eklenen değişkenler Role Tablosuna eklenecek ve istedğimiz Role'leri  ekleyebiliriz 

        public string Description { get; set; }
        public ApplicationRole()
        {

        }
        public ApplicationRole(string rolename, string description)
        {
            this.Description = description;

        }
    
    }
}
