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
    public class WeatherSQLiteDataBroker :
        SQLiteDataBroker<SQLiteWeatherDbContext>
    {
        public WeatherSQLiteDataBroker(IConfiguration configuration, IDbContextFactory<SQLiteWeatherDbContext> dbContext) : base(configuration, dbContext) { }
    }
}
