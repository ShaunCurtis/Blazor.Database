using Blazor.Database.Data;

namespace Blazor.Database.Services
{
    public class WeatherForecastControllerService : FactoryControllerService<WeatherForecast, LocalWeatherDbContext>, IFactoryControllerService<WeatherForecast, LocalWeatherDbContext>
    {
    }
}
