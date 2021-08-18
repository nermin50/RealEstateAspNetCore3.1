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
        public DataContext(DbContextOptions <DataContext> options) : base(options)
        {

        }
        // Model ve database ile bağlantı kısmı 
        // public DbSet<ModelAdı> tabloadı {get; set;}

        public DbSet<Status> Status { get; set; }

        public DbSet<Tip> Tips { get; set; }

        public DbSet<Advertisement> advertisements { get; set; }
        public DbSet<AdvPhoto> advPhotos { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<District> districts { get; set; }

        public DbSet<Neighborhood> neighborhoods { get; set; }


    }
}
