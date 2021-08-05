/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Database.Data;
using Blazr.SPA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blazr.Database.Brokers
{
    public class WeatherInMemoryDataBroker :
        InMemoryDataStoreBroker<InMemoryWeatherDataStore>
    {
        public WeatherInMemoryDataBroker(IInMemoryDataStore dataContext) : base(dataContext) { }
    }
}
