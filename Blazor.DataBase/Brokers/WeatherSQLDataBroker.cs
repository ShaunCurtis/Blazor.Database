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
    public class WeatherSQLDataBroker :
        ServerDataBroker<LocalWeatherDbContext>
    {
        public WeatherSQLDataBroker(IConfiguration configuration, IDbContextFactory<LocalWeatherDbContext> dbContext) : base(configuration, dbContext) { }

    }
}
