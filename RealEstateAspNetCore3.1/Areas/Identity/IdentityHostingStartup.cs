using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateAspNetCore3._1.Identity;

[assembly: HostingStartup(typeof(RealEstateAspNetCore3._1.Areas.Identity.IdentityHostingStartup))]
namespace RealEstateAspNetCore3._1.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        // ıdenitity Konfigarasyonu burda yapıldır 
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}