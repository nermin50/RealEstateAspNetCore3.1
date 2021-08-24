using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Identity
{
    // apllication user ve rol birbirne bağlar  ve vertabınının bağlantısı sağlar 
    public class IdentityDataContext : IdentityDbContext<ApplicationUser , ApplicationRole , string>
    {
        
        public IdentityDataContext(DbContextOptions options) : base(options)
        {

           
        }
    }
}
