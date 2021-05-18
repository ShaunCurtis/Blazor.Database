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
    public class WeatherInMemoryDataBroker :
        InMemoryDataBroker<InMemoryWeatherDbContext>
    {
        public WeatherInMemoryDataBroker(IConfiguration configuration, IDbContextFactory<InMemoryWeatherDbContext> dbContext) : base(configuration, dbContext) { }

    }
}
