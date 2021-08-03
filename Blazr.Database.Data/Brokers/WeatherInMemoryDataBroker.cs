/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Database.Data;
using Blazr.SPA.Brokers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blazr.Database.Brokers
{
    public class WeatherInMemoryDataBroker :
        InMemoryDataBroker<InMemoryWeatherDbContext>
    {
        public WeatherInMemoryDataBroker(IConfiguration configuration, IDbContextFactory<InMemoryWeatherDbContext> dbContext) : base(configuration, dbContext) { }
    }
}
