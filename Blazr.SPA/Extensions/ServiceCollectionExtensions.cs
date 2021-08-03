/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Blazr.SPA.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorSPA(this IServiceCollection services)
        {
            services.AddSingleton<RouteViewService>();
            services.AddScoped<EditStateService>();
            return services;
        }
    }
}
