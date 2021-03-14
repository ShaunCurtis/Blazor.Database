using Blazor.Database.Data;
using Blazor.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Database.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Singleton service for the Server Side version of WeatherForecast Data Service 
            // Dummy service produces a new recordset each time the application runs 

            // services.AddSingleton<IFactoryDataService<WeatherForecastDbContext>, FactoryServerDataService<WeatherForecastDbContext>>();
             services.AddSingleton<IFactoryDataService<LocalWeatherDbContext>, FactoryServerDataService<LocalWeatherDbContext>>();
            services.AddScoped<WeatherForecastControllerService>();

            //var dbContext = "Data Source=:memory:";
            //services.AddDbContext<InMemoryWeatherDbContext>(options => options.UseSqlite(dbContext), ServiceLifetime.Singleton);
            //services.AddSingleton<IWeatherDataService, WeatherServerDataService>();
            //services.AddScoped<WeatherControllerService>();

            // Factory for building the DBContext 
            var dbContext = configuration.GetValue<string>("Configuration:DBContext");
            services.AddDbContextFactory<LocalWeatherDbContext>(options => options.UseSqlServer(dbContext), ServiceLifetime.Singleton);
            return services;
        }
    }
}
