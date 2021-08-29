using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealEstateAspNetCore3._1.Identity;
using RealEstateAspNetCore3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAspNetCore3._1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Net Core 'de Database'e bağlantyı sağlayan kod : Option parametre'e üzerinden defaultCon string'ini burda 
            // alıp DataContext sınınıfa gönderir
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultCon")));
            // Identity bağlantısına konfigrasyon yapıyor 
            services.AddDbContext<IdentityDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            // Bütün User ve Rol arasındaki ilişkleri sağalr 
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityDataContext>();

            // RunCompilatiion : yani kod'da eğer bir değişiklik yaparsak (HTML'de) kodların değişiklerini kaydedip sadece sayfayı yenilenmye ihtiyacımız var
            // yeniden çalıştırmasına gerek yok 
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRazorPages();

            //yetkilendirme işlemi  
            services.AddMvc();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddAuthorization();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Identity/Account/Login");

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env ,
            UserManager<ApplicationUser> userManger, RoleManager<ApplicationRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // proje ilk başladığında Seed fonksyonunu çalıştırır 
            IdentityInitilizer.Seed(userManger, roleManager);

            app.UseStaticFiles();

            app.UseRouting();

            
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseSession();
           
           app.UseEndpoints(endpoints =>
            {
                //Kontrollerin haritalaması 
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
