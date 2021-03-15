using Blazor.Database.Data;
using Blazor.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Database.WASM.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFactoryDataService, FactoryWASMDataService>();
            services.AddScoped<WeatherForecastControllerService>();

            return services;
        }
    }
}
