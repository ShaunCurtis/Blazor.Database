/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Services;
using Blazor.SPA.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Database.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWASMApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFactoryDataService, FactoryWASMDataService>();
            services.AddScoped<WeatherForecastControllerService>();

            return services;
        }
    }
}
