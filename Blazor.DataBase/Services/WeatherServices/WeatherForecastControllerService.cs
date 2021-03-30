/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.SPA.Components;
using Blazor.SPA.Services;
using Blazor.SPA.Data;
using Blazor.Database.Data;

namespace Blazor.Database.Services
{
    public class WeatherForecastControllerService : FactoryControllerService<WeatherForecast>, IFactoryControllerService<WeatherForecast>
    {
        public WeatherForecastControllerService(IFactoryDataService factoryDataService) : base(factoryDataService) { }
    }
}
