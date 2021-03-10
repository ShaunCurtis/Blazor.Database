using Blazor.Database.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blazor.Database.Services;

namespace Blazor.Database.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services){
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllersWithViews();
            var dbContext = "Data Source=:memory:";
            services.AddDbContext<WeatherDbContext>(options => options.UseSqlite(dbContext), ServiceLifetime.Singleton);
            services.AddSingleton<IWeatherDataService, WeatherServerDataService>();
            services.AddScoped<WeatherControllerService>();

            services.AddSingleton<WeatherForecastService>();
            // Server Side Blazor doesn't register HttpClient by default
            // Thanks to Robin Sue - Suchiman https://github.com/Suchiman/BlazorDualMode
            if (!services.Any(x => x.ServiceType == typeof(HttpClient)))
            {
                // Setup HttpClient for server side in a client side compatible fashion
                services.AddScoped<HttpClient>(s =>
                {
                    // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
                    var uriHelper = s.GetRequiredService<NavigationManager>();
                    return new HttpClient
                    {
                        BaseAddress = new Uri(uriHelper.BaseUri)
                    };
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/WASM"), app1 =>
            {
                app1.UseBlazorFrameworkFiles("/WASM");
                app1.UseRouting();
                app1.UseEndpoints(endpoints =>
                {
                    endpoints.MapFallbackToPage("/red/{*path:nonfile}", "/_WASM");
                });
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapRazorPages();
                endpoints.MapFallbackToPage("/Server/{*path:nonfile}","/_Host");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
