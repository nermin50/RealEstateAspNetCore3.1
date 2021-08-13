using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Identity
{
    public static class IdentityInitilizer
    {
        // süper admin profilini veritabanında olup olmadığını kontrol eder  eğer süper adminin mailini bulmazsa yeni bir profil oluşturur
        //profili oluşturdukan sonra şifreyi ekler . ondan sonra rölunu ekler 
        public static void Seed(UserManager<ApplicationUser> userManger, RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManger);
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManger)
        {
            // tabloda mail aadresi olup olmadığını kontrol eder  eğer yoksa ->>>
            if (userManger.FindByEmailAsync("nermin@gmail.com").Result == null)
            {
                /// --> yeni süper admin oluşturur
                var user = new ApplicationUser
                {
                    UserName = "nermin",
                    Email = "nermin@gmail.com"
                };

                //şifreyi ekler
                var result = userManger.CreateAsync(user, "Admin1!").Result;
                if (result.Succeeded)
                {
                    // rölunu ekler 
                    userManger.AddToRoleAsync(user, "admin");
                }
            }

            /// ikinci normal kullanıcı oluştur 
             // tabloda yeni kullanıcının  mail aadresini olup olmadığını kontrol eder  eğer yoksa ->>>
            if (userManger.FindByEmailAsync("marah@gmail.com").Result == null)
            {
                /// --> yeni normal oluşturur
                var user = new ApplicationUser
                {
                    UserName = "marah",
                    Email = "marah@gmail.com"
                };

                //şifreyi ekler
                var result = userManger.CreateAsync(user, "User1!").Result;
                if (result.Succeeded)
                {
                    // rölunu ekler 
                    userManger.AddToRoleAsync(user, "user");
                }
            }
        }


        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            // burda Role tablousunda rol yoksa yeni roller oluşturur 
            if (!roleManager.RoleExistsAsync("admin").Result)
            {
                // yeni admin rölünü oluştur
                var role = new ApplicationRole
                {
                    Name = "admin" ,
                   Description="admin rölü süper admin"

                };
                // rölu ekle
                var result = roleManager.CreateAsync(role);
            }
            if (!roleManager.RoleExistsAsync("user").Result)
            {
                //dğier kullanıcılar için user rölünü ekle
                var role = new ApplicationRole
                {
                    Name = "user",
                    Description = "normal user  rölü "

                };
                var result = roleManager.CreateAsync(role).Result;
            }


        }

    }
}
