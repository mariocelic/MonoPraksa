using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonoPraksa.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace MonoPraksa
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PraksaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<PraksaContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();


            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "Resources";
            });

            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(
                opt =>
                {
                    var susupportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("hr"),
                        new CultureInfo("en")
                    };

                    opt.DefaultRequestCulture = new RequestCulture("hr");
                    opt.SupportedCultures = susupportedCultures;
                    opt.SupportedUICultures = susupportedCultures;
                });

            services.AddControllersWithViews(); 
            services.AddRazorPages();

        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            //var supportedCultures = new [] { "hr", "en"};
            //var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
            //    .AddSupportedCultures(supportedCultures)
            //    .AddSupportedUICultures(supportedCultures);

            //app.UseRequestLocalization(localizationOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
