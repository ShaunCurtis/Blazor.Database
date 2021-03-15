/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Data;

namespace Blazor.Database.Services
{
    public class WeatherForecastControllerService : FactoryControllerService<WeatherForecast>
    {
        public WeatherForecastControllerService(IFactoryDataService factoryDataService) : base(factoryDataService) { }
    }
}
