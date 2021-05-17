/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Brokers;
using Blazor.Database.Data;
using Blazor.Database.Services;
using Blazor.SPA.Brokers;
using Blazor.SPA.Connectors;
using Blazor.SPA.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blazor.Database.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWASMApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDataBroker, APIDataBroker>();
            AddCommonServices(services);

            return services;
        }
        public static IServiceCollection AddServerApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Local DB Setup
            var dbContext = configuration.GetValue<string>("Configuration:DBContext");
            services.AddDbContextFactory<LocalWeatherDbContext>(options => options.UseSqlServer(dbContext), ServiceLifetime.Singleton);
            services.AddSingleton<IDataBroker, WeatherForecastSQLDataBroker>();
            AddCommonServices(services);

            return services;
        }

        public static IServiceCollection AddInMemoryApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            // In Memory DB Setup
            var memdbContext = "Data Source=:memory:";
            services.AddDbContextFactory<InMemoryWeatherDbContext>(options => options.UseSqlite(memdbContext), ServiceLifetime.Singleton);
            services.AddSingleton<IDataBroker, WeatherForecastInMemoryDataBroker>();
            AddCommonServices(services);

            return services;
        }

        private static void AddCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<RouteViewService>();
            services.AddScoped<ILogger, Logger<LoggingBroker>>();
            services.AddScoped<ILoggingBroker, LoggingBroker>();
            services.AddScoped<IDateTimeBroker, DateTimeBroker>();
            services.AddScoped<IDataServiceConnector, ModelDataServiceConnector>();
            services.AddScoped<WeatherForecastViewService>();
            services.AddSingleton<RandomNumberService>();
        }
    }
}
