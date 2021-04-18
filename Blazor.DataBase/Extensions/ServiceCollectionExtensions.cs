/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Services;
using Blazor.SPA.Services;
using Microsoft.Extensions.DependencyInjection;
using Blazor.Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blazor.Database.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWASMApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFactoryDataService, FactoryWASMDataService>();
            AddCommonServices(services);

            return services;
        }
        public static IServiceCollection AddServerApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Local DB Setup
            var dbContext = configuration.GetValue<string>("Configuration:DBContext");
            services.AddDbContextFactory<LocalWeatherDbContext>(options => options.UseSqlServer(dbContext), ServiceLifetime.Singleton);
            services.AddSingleton<IFactoryDataService, LocalDatabaseDataService>();
            AddCommonServices(services);

            return services;
        }

        public static IServiceCollection AddInMemoryApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            // In Memory DB Setup
            var memdbContext = "Data Source=:memory:";
            services.AddDbContextFactory<InMemoryWeatherDbContext>(options => options.UseSqlite(memdbContext), ServiceLifetime.Singleton);
            services.AddSingleton<IFactoryDataService, TestDatabaseDataService>();
            AddCommonServices(services);

            return services;
        }

        private static void AddCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<RouteViewService>();
            services.AddScoped<WeatherForecastControllerService>();
            services.AddScoped<WeatherForecastControllerService>();
            services.AddSingleton<RandomNumberService>();
        }
    }
}
