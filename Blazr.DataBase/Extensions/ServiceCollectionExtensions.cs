/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Database.Brokers;
using Blazr.Database.Core;
using Blazr.Database.Data;
using Blazr.SPA.Brokers;
using Blazr.SPA.Connectors;
using Blazr.SPA.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blazr.Database.Extensions
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
            services.AddDbContextFactory<MSSQLWeatherDbContext>(options => options.UseSqlServer(dbContext), ServiceLifetime.Singleton);
            services.AddSingleton<IDataBroker, WeatherSQLDataBroker>();
            AddCommonServices(services);

            return services;
        }

        public static IServiceCollection AddInMemoryApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            // In Memory DB Setup
            var memdbContext = "Data Source=:memory:";
            services.AddDbContextFactory<InMemoryWeatherDbContext>(options => options.UseSqlite(memdbContext), ServiceLifetime.Singleton);
            services.AddSingleton<IDataBroker, WeatherInMemoryDataBroker>();
            AddCommonServices(services);

            return services;
        }

        private static void AddCommonServices(this IServiceCollection services)
        {
            services.AddBlazorSPA();
            services.AddScoped<ILogger, Logger<LoggingBroker>>();
            services.AddScoped<ILoggingBroker, LoggingBroker>();
            services.AddScoped<IDateTimeBroker, DateTimeBroker>();
            services.AddScoped<IDataServiceConnector, ModelDataServiceConnector>();
            services.AddScoped<WeatherForecastViewService>();
        }
    }
}
