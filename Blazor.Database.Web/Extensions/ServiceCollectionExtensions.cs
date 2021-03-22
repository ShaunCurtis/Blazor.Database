﻿using Blazor.Database.Data;
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

            // Local DB Setup
            //var dbContext = configuration.GetValue<string>("Configuration:DBContext");
            //services.AddDbContextFactory<LocalWeatherDbContext>(options => options.UseSqlServer(dbContext), ServiceLifetime.Singleton);
            //services.AddSingleton<IFactoryDataService, LocalDatabaseDataService>();

            // In Memory DB Setup
            var memdbContext = "Data Source=:memory:";
            services.AddDbContextFactory<InMemoryWeatherDbContext>(options => options.UseSqlite(memdbContext), ServiceLifetime.Singleton);
            services.AddSingleton<IFactoryDataService, TestDatabaseDataService>();

            services.AddScoped<WeatherForecastControllerService>();

            return services;
        }
    }
}