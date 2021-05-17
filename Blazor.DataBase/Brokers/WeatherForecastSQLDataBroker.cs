/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Blazor.Database.Data;
using Blazor.SPA.Brokers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blazor.Database.Brokers
{
    public class WeatherForecastSQLDataBroker :
        ServerDataBroker<LocalWeatherDbContext>
    {
        public WeatherForecastSQLDataBroker(IConfiguration configuration, IDbContextFactory<LocalWeatherDbContext> dbContext) : base(configuration, dbContext) { }

    }
}
