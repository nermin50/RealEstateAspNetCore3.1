using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1.Models
{
    public class DataContext : DbContext
    {
        //Database Connection 
        public DataContext(DbContextOptions options) : base(options)
        {

        }


    }
}
